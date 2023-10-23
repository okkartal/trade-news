using System.Text.Json.Serialization;

namespace Shared.Entities;
public class Publisher
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("homepage_url")]
    public Uri HomepageUrl { get; set; }

    [JsonPropertyName("logo_url")]
    public Uri LogoUrl { get; set; }

    [JsonPropertyName("favicon_url")]
    public Uri FaviconUrl { get; set; }
}
