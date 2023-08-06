
namespace UrlShortner.Models;
[GenerateSerializer]
public record UrlDetails
{
    public string FullUrl { get; set; }
    public string ShortenedRouteSegment { get; set; }
}