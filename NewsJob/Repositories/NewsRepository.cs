using MongoDB.Driver;
using Shared.Entities;

namespace NewsJob.Repositories;

public class NewsRepository : INewsRepository
{
    private readonly IMongoCollection<News> _newsCollection;

    public NewsRepository(IMongoDatabase database)
    {
        _newsCollection = database.GetCollection<News>(nameof(News));
    }

    public async Task SaveNews(IEnumerable<News> news)
    {
        await _newsCollection.InsertManyAsync(news);
    }
}