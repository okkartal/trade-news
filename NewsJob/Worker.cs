using System.Text.Json;
using NewsJob.Repositories;
using Shared.Entities;

namespace NewsJob;

public class Worker : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<Worker> _logger;
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
        var apiUrl = $"{_configuration["JobProperties:ApiUrl"]}&apiKey={_configuration["JobProperties:ApiKey"]}";
        var delayInMiliSeconds = Convert.ToInt32(_configuration["JobProperties:DelayMiliSeconds"]);
        using var httpClient = _httpClientFactory.CreateClient();

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            try
            {
                var response = await httpClient.GetAsync(apiUrl, stoppingToken);

                if (response.IsSuccessStatusCode)
                {
                    var newsData = await response.Content.ReadAsStringAsync(stoppingToken);

                    var polygonResponse = JsonSerializer.Deserialize<PolygonResponse>(newsData);

                    if (polygonResponse?.Results.Length > 0) await SaveNews(polygonResponse.Results);
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