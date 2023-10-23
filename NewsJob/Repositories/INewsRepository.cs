using Shared.Entities;

namespace NewsJob.Repositories;

public interface INewsRepository
{
    Task SaveNews(IEnumerable<News> news);
}