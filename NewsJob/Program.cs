using MongoDB.Driver;
using NewsJob;
using NewsJob.Repositories;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IMongoDatabase>(provider =>
        {
            var configuration = provider.GetService<IConfiguration>();
            var mongoClient = new MongoClient(configuration?["MongoDbSettings:Uri"]);
            return mongoClient.GetDatabase(configuration?["MongoDbSettings:Database"]);
        });

        services.AddSingleton<INewsRepository, NewsRepository>();
        services.AddHostedService<Worker>();
        services.AddHttpClient();
    })
    .Build();

host.Run();