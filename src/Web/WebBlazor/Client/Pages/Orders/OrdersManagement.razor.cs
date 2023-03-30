
namespace WebBlazor.Client.Pages.Orders;

[Authorize]
public partial class OrdersManagement
{
    private List<OrderDTO> orders = new();
    private bool errorReceived;
    private string errorMessage;
    private string userId;

    [Inject]
    private IOrderingService OrderingService { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    protected override async Task OnInitializedAsync()
    {
        userId = (await AuthenticationStateTask).User.GetSub();
        await LoadData();
    }

    private async Task LoadData()
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

    private async Task OrderProcess(string orderId, ChangeEventArgs e)
    {
        if (OrderProcessActionDTO.Ship.Code == e.Value.ToString())
        {
            try
            {
                await OrderingService.ShipOrder(orderId);
                await LoadData();
                errorMessage = null;
            }
            catch (OrderDomainException ex)
            {
                errorMessage = ex.Message;
            }
        }
    }
}
