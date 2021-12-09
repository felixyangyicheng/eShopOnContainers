
namespace WebBlazor.Client.Pages.Orders;

[Authorize]
public partial class OrdersDetail
{
    private OrderDTO order = new();

    [Inject]
    private IOrderingService OrderingService { get; set; }

    [Parameter]
    public int Id { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var userId = (await AuthenticationStateTask).User.GetSub();
        await GetOrder(userId);
    }

    private async Task GetOrder(string userId) =>
        order = await OrderingService.GetOrder(userId, Id.ToString(CultureInfo.InvariantCulture));
}
