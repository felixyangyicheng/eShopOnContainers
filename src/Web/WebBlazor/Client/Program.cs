
var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("app");

using var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };

var appSettings = await http.GetFromJsonAsync<AppSettings>("home/config");

builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
{
    ["PurchaseUrl"] = appSettings.PurchaseUrl,
    ["MarketingUrl"] = appSettings.MarketingUrl,
    ["SignalrHubUrl"] = appSettings.SignalrHubUrl,
    ["ActivateCampaignDetailFunction"] = appSettings.ActivateCampaignDetailFunction
});

builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.ClientId = "blazor";
    options.ProviderOptions.Authority = appSettings.IdentityUrl;
    options.ProviderOptions.PostLogoutRedirectUri = appSettings.CallBackUrl;
    options.ProviderOptions.ResponseType = "id_token token";
    options.ProviderOptions.DefaultScopes.Add("orders");
    options.ProviderOptions.DefaultScopes.Add("basket");
    options.ProviderOptions.DefaultScopes.Add("webshoppingagg");
    options.ProviderOptions.DefaultScopes.Add("orders.signalrhub");
});

builder.Services.AddHttpClientServices();

builder.Services.AddScoped<IEventService, EventService>();

await builder.Build().RunAsync();
