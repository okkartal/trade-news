using NewsJob.Repositories;
using Shared.Entities;
using System.Text.Json;

namespace NewsJob;
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly INewsRepository _newsRepository;

    public Worker(ILogger<Worker> logger,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        INewsRepository newsRepository)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _newsRepository = newsRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        string apiUrl = $"{_configuration["JobProperties:ApiUrl"]}&apiKey={_configuration["JobProperties:ApiKey"]}";
        int delayInMiliSeconds = Convert.ToInt32(_configuration["JobProperties:DelayMiliSeconds"]);
        using HttpClient httpClient = _httpClientFactory.CreateClient();

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            try
            {

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string newsData = await response.Content.ReadAsStringAsync();

                    var polygonResponse = JsonSerializer.Deserialize<PolygonResponse>(newsData);

                    if (polygonResponse?.Results?.Length > 0)
                    {
                        await SaveNews(polygonResponse.Results);
                    }
                }
                else
                {
                    Console.WriteLine($"Failed to fetch news.Status Code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured: {ex.Message}");
            }

            await Task.Delay(delayInMiliSeconds, stoppingToken);
        }
    }

    private async Task SaveNews(News[] news)
    {
        await _newsRepository.SaveNews(news);
    }
}
