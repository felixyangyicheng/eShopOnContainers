
namespace WebBlazor.Client.Services;

public class OrderingService : IOrderingService
{
    private readonly HttpClient _httpClient;
    private readonly string _remoteServiceBaseUrl;
    private static readonly JsonSerializerOptions jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public OrderingService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _remoteServiceBaseUrl = $"{configuration["PurchaseUrl"]}/o/api/v1/orders";
    }

    public OrderDTO MapUserInfoIntoOrder(ClaimsPrincipal user, OrderDTO order)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (order is null)
        {
            throw new ArgumentNullException(nameof(order));
        }

        var expirationSplit = user.GetExpiration().Split('/');

        order.City = user.GetCity();
        order.Street = user.GetStreet();
        order.State = user.GetState();
        order.Country = user.GetCountry();
        order.ZipCode = user.GetZipCode();

        order.CardNumber = user.GetCardNumber();
        order.CardHolderName = user.GetCardHolderName();
        order.CardExpiration = new DateTime(int.Parse("20" + expirationSplit[1], CultureInfo.InvariantCulture), int.Parse(expirationSplit[0], CultureInfo.InvariantCulture), 1);
        order.CardSecurityNumber = user.GetSecurityNumber();

        return order;
    }

    public BasketCheckoutDTO MapOrderToBasket(OrderDTO order)
    {
        if (order is null)
        {
            throw new ArgumentNullException(nameof(order));
        }

        order.CardExpirationApiFormat();

        return new()
        {
            City = order.City,
            Street = order.Street,
            State = order.State,
            Country = order.Country,
            ZipCode = order.ZipCode,
            CardNumber = order.CardNumber,
            CardHolderName = order.CardHolderName,
            CardExpiration = order.CardExpiration,
            CardSecurityNumber = order.CardSecurityNumber,
            CardTypeId = 1,
            Buyer = order.Buyer,
            RequestId = order.RequestId
        };
    }

    public async Task<List<OrderDTO>> GetMyOrders(string userId)
    {
        var uri = API.Order.GetAllMyOrders(_remoteServiceBaseUrl);

        var responseString = await _httpClient.GetStringAsync(new Uri(uri));

        var response = JsonSerializer.Deserialize<List<OrderDTO>>(responseString, jsonSerializerOptions);

        return response;
    }

    public async Task CancelOrder(string orderId)
    {
        var uri = API.Order.CancelOrder(_remoteServiceBaseUrl);

        var order = new OrderDTO { OrderNumber = orderId };

        using var orderContent = new StringContent(JsonSerializer.Serialize(order), Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync(new Uri(uri), orderContent);

        if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            throw new Exception("Error cancelling order, try later.");
        }

        response.EnsureSuccessStatusCode();
    }

    public async Task<OrderDTO> GetOrder(string userId, string orderId)
    {
        var uri = API.Order.GetOrder(_remoteServiceBaseUrl, orderId);

        var responseString = await _httpClient.GetStringAsync(new Uri(uri));

        var response = JsonSerializer.Deserialize<OrderDTO>(responseString, jsonSerializerOptions);

        return response;
    }

    public async Task ShipOrder(string orderId)
    {
        var uri = API.Order.ShipOrder(_remoteServiceBaseUrl);

        var order = new OrderDTO { OrderNumber = orderId };

        using var orderContent = new StringContent(JsonSerializer.Serialize(order), Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync(new Uri(uri), orderContent);

        if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            throw new Exception("Error in ship order process, try later.");
        }

        response.EnsureSuccessStatusCode();
    }
}
