using System.Text.Json.Serialization;

namespace Shared.Entities;

public class News
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("publisher")]
    public Publisher Publisher { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("author")]
    public string Author { get; set; }

    [JsonPropertyName("published_utc")]
    public DateTimeOffset PublishedUtc { get; set; }

    [JsonPropertyName("article_url")]
    public Uri ArticleUrl { get; set; }

    [JsonPropertyName("tickers")]
    public string[] Tickers { get; set; }

    [JsonPropertyName("amp_url")]
    public Uri AmpUrl { get; set; }

    [JsonPropertyName("image_url")]
    public Uri ImageUrl { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("keywords")]
    public string[] Keywords { get; set; }
}

