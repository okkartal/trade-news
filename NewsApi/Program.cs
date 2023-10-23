using MongoDB.Driver;
using NewsApi.Middlewares;
using NewsApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMongoDatabase>(provider =>
{
    var configuration = provider.GetService<IConfiguration>();
    var mongoClient = new MongoClient(configuration?["MongoDbSettings:Uri"]);
    return mongoClient.GetDatabase(configuration?["MongoDbSettings:Database"]);
});

// Add services to the container.
builder.Services.AddSingleton<INewsRepository, NewsRepository>();

var app = builder.Build();

app.UseMiddleware<ApiKeyMiddleware>();

app.MapGet("/AllNews", async (INewsRepository newsRepository) =>
{
    var result = await newsRepository.GetAllNews();
    return Results.Ok(result);
});

app.MapGet("/AllNewsFromTodayToSpecifiedDay/{day:int}", async (INewsRepository newsRepository, int day) =>
{
    var result = await newsRepository.GetAllNewsFromFromTodayToSpecifiedDay(day);
    return Results.Ok(result);
});

app.MapGet("/AllNewsPerInstrumentWithLimit/{instrument}/{limit:int}",
    async (INewsRepository newsRepository, string instrument, int limit) =>
    {
        var result = await newsRepository.GetAllNewsPerInstrumentWithLimit(instrument, limit);
        return Results.Ok(result);
    });

app.MapGet("/AllNewsContainsText/{text}", async (INewsRepository newsRepository, string text) =>
{
    var result = await newsRepository.GetAllNewsWithSpecifiedTexT(text);
    return Results.Ok(result);
});


app.MapGet("/LatestNews", async (INewsRepository newsRepository) =>
{
    var result = await newsRepository.GetLatestNews();
    return Results.Ok(result);
});


app.Run();