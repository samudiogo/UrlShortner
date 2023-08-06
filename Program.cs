using Grains;
using Microsoft.AspNetCore.Http.Extensions;
using static UrlShortner.Utils.RouteSegment;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder.UseLocalhostClustering();
    siloBuilder.AddMemoryGrainStorage("urls");
});
var app = builder.Build();

var grainFactory = app.Services.GetRequiredService<IGrainFactory>();

app.MapGet("/shorten/{*path}",
async (IGrainFactory grains, HttpRequest request, string path) =>
{
    var shortenedRouteSegment = GetRouteSegment("X");
    var shortenGrain = grains.GetGrain<IUrlShortenerGrain>(shortenedRouteSegment);
    await shortenGrain.SetUrl(path);
    var resultBuilder = new UriBuilder(request.GetEncodedUrl())
    {
        Path = $"/go/{shortenedRouteSegment}"
    };

    return Results.Ok(resultBuilder.Uri);
}
);


app.MapGet("/go/{shortenedRouteSegment}",
async (IGrainFactory grains, string shortenedRouteSegment) =>
{
    var shortenGrain = grains.GetGrain<IUrlShortenerGrain>(shortenedRouteSegment);

    var url = await shortenGrain.GetUrl();

    return Results.Redirect(url);
}
);

app.Run();
