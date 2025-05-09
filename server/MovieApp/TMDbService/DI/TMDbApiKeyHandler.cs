using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using TMDbService.Config;

namespace TMDbService.DI;

public class TMDbApiKeyHandler : DelegatingHandler
{
    private readonly TMDbSettings _settings;

    public TMDbApiKeyHandler(IOptions<TMDbSettings> settings)
    {
        _settings = settings.Value;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var originalUri = request.RequestUri!.ToString();

        var updatedUri = QueryHelpers.AddQueryString(originalUri, "api_key", _settings.ApiKey);

        request.RequestUri = new Uri(updatedUri);
        return base.SendAsync(request, cancellationToken);
    }
}