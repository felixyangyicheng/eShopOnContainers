
namespace WebBlazor.Client.Pages.Basket;

[Authorize]
public partial class Basket
{
    private bool errorUpdate;
    private BasketDTO basket = new();
    private string userId;

    [Inject]
    private IBasketService BasketService { get; set; }

    [Inject]
    private NavigationManager Navigation { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    private bool HasItemWithInvalidQuantity =>
        basket.Items.Any(x => x.Quantity < 1);

    protected override async Task OnInitializedAsync()
    {
        userId = (await AuthenticationStateTask).User.GetSub();
        basket = await BasketService.GetBasket(userId);
    }

    private async Task<bool> Update()
    {
        try
        {
            if (HasItemWithInvalidQuantity)
            {
                errorUpdate = false;
                return false;
            }
            await BasketService.UpdateBasket(basket);
            errorUpdate = false;
            return true;
        }
        catch (Exception)
        {
            errorUpdate = true;
            return false;
        }
    }

    private async Task CheckOut()
    {
        if (await Update())
        {
            Navigation.NavigateTo("ordersnew");
        }
    }

    private async Task ItemQuantityChanged(BasketItemDTO item, int quantity)
    {
        item.Quantity = quantity > 0 ? quantity : 1;
        await Update();
    }

    private async Task DeleteItem(string id)
    {
        var itemToRemove = basket.Items.FirstOrDefault(x => x.Id == id);
        if (itemToRemove == null)
        {
            return;
        }
        basket.Items.Remove(itemToRemove);
        await Update();
    }
}
