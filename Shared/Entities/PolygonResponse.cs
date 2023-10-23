using System.Text.Json.Serialization;

namespace Shared.Entities;

public class PolygonResponse
{
    [JsonPropertyName("results")] public News[] Results { get; set; }

    [JsonPropertyName("status")] public string Status { get; set; }

    [JsonPropertyName("request_id")] public string RequestId { get; set; }

    [JsonPropertyName("count")] public long Count { get; set; }

    [JsonPropertyName("next_url")] public Uri NextUrl { get; set; }
}