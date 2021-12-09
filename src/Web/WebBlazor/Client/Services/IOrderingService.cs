
namespace WebBlazor.Client.Services;

public interface IOrderingService
{
    OrderDTO MapUserInfoIntoOrder(ClaimsPrincipal user, OrderDTO order);

    BasketCheckoutDTO MapOrderToBasket(OrderDTO order);

    Task<List<OrderDTO>> GetMyOrders(string userId);

    Task CancelOrder(string orderId);

    Task<OrderDTO> GetOrder(string userId, string orderId);

    Task ShipOrder(string orderId);
}
