using Shared.Entities;

namespace NewsApi.Repositories;
public interface INewsRepository
{
    Task<IReadOnlyCollection<News>> GetAllNews();

    Task<IReadOnlyCollection<News>> GetAllNewsFromFromTodayToSpecifiedDay(int specifiedDay);

    Task<IReadOnlyCollection<News>> GetAllNewsPerInstrumentWithLimit(string instrument, int limit);

    Task<IReadOnlyCollection<News>> GetAllNewsWithSpecifiedTexT(string text);

    Task<IReadOnlyCollection<News>> GetLatestNews();
}

