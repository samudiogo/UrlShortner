using Grains;
using Orleans.Runtime;
using UrlShortner.Models;

namespace UrlShortner.Grains;

public class UrlShortenerGrain : Grain, IUrlShortenerGrain
{
    private IPersistentState<UrlDetails> _cache;

    public UrlShortenerGrain([PersistentState(stateName: "url", storageName: "urls")]
    IPersistentState<UrlDetails> state)
    {
        _cache = state;
    }

    public async Task SetUrl(string fullUrl)
    {
        _cache.State = new UrlDetails() { ShortenedRouteSegment = this.GetPrimaryKeyString(), FullUrl = fullUrl };
        await _cache.WriteStateAsync();
    }
    public Task<string> GetUrl() => Task.FromResult(_cache.State.FullUrl);

}
