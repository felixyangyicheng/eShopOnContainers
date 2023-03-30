
namespace WebBlazor.Client.Pages.Orders;

[Authorize]
public partial class Orders : IAsyncDisposable
{
    private bool errorReceived;
    private string errorMessage;
    private string userId;
    private HubConnection hubConnection;
    private List<OrderDTO> orders = new();

    [Inject]
    private IOrderingService OrderingService { get; set; }

    [Inject]
    private IConfiguration Configuration { get; set; }

    [Inject]
    private IAccessTokenProvider TokenProvider { get; set; }

    [Inject]
    private IJSRuntime JsRuntime { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    protected override async Task OnInitializedAsync()
    {
        userId = (await AuthenticationStateTask).User.GetSub();
        await GetOrders();
        await InitSignalR();
    }

    private async Task InitSignalR()
    {
        var tokenResult = await TokenProvider.RequestAccessToken(new AccessTokenRequestOptions
        {
            Scopes = new[] { "orders.signalrhub" }
        });
        if (!tokenResult.TryGetToken(out var token))
        {
            return;
        }
        hubConnection = new HubConnectionBuilder()
            .WithUrl($"{Configuration["SignalrHubUrl"]}/hub/notificationhub", options =>
                options.AccessTokenProvider = () => Task.FromResult(token.Value))
            .WithAutomaticReconnect()
            .Build();
        hubConnection.On<HubMessage>("UpdatedOrderState", async message =>
        {
            await GetOrders();
            StateHasChanged();
            await JsRuntime.InvokeVoidAsync("showToastrNotification", message.Status, message.OrderId);
        });
        await hubConnection.StartAsync();
    }

    private async Task GetOrders()
    {
        try
        {
            orders = await OrderingService.GetMyOrders(userId);
            errorReceived = false;
        }
        catch (Exception)
        {
            errorReceived = true;
            throw;
        }
    }

    private async Task CancelOrder(string orderNumber)
    {
        try
        {
            await OrderingService.CancelOrder(orderNumber);
            errorMessage = null;
        }
        catch (OrderDomainException ex)
        {
            errorMessage = ex.Message;
        }
    }

    public async ValueTask DisposeAsync() =>
        await hubConnection.DisposeAsync();
}
