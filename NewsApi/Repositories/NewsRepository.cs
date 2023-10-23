using MongoDB.Driver;
using Shared.Entities;

namespace NewsApi.Repositories;
public class NewsRepository : INewsRepository
{
    private readonly IMongoCollection<News> _newsCollection;
    private readonly FilterDefinitionBuilder<News> _filterBuilder = Builders<News>.Filter;
    public NewsRepository(IMongoDatabase database)
    {
        _newsCollection = database.GetCollection<News>(nameof(News));
    }

    public async Task<IReadOnlyCollection<News>> GetAllNews()
    {
        return await _newsCollection.Find(_filterBuilder.Empty).ToListAsync();
    }

    public async Task<IReadOnlyCollection<News>> GetAllNewsFromFromTodayToSpecifiedDay(int specifiedDay)
    {
        return await _newsCollection.Find(x => x.PublishedUtc >= DateTime.Now.AddDays(-specifiedDay)).ToListAsync();
    }

    public async Task<IReadOnlyCollection<News>> GetAllNewsPerInstrumentWithLimit(string instrument, int limit)
    {
        return await _newsCollection.Find(x => x.Tickers.Contains(instrument)).Limit(limit).ToListAsync();
    }

    public async Task<IReadOnlyCollection<News>> GetAllNewsWithSpecifiedTexT(string text)
    {
        return await _newsCollection.Find(x => x.Title.Contains(text)).ToListAsync();
    }

    public async Task<IReadOnlyCollection<News>> GetLatestNews()
    {
        return await _newsCollection.Find(_filterBuilder.Empty).SortByDescending(x => x.PublishedUtc).ToListAsync();
    }
}

