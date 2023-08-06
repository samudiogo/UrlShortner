namespace UrlShortner.Utils;

public static class RouteSegment
{
    public static string GetRouteSegment(string format)
    {
        return Guid.NewGuid().GetHashCode().ToString(format);
    }
}
