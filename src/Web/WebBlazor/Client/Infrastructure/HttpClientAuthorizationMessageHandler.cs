
namespace WebBlazor.Client.Infrastructure;

public class HttpClientAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public HttpClientAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigationManager, IConfiguration configuration)
        : base(provider, navigationManager)
    {
        if (configuration is null) throw new ArgumentNullException(nameof(configuration));

        ConfigureHandler(
            authorizedUrls: new[] { configuration["PurchaseUrl"] },
            scopes: new[] { "basket", "orders" }
        );
    }
}
