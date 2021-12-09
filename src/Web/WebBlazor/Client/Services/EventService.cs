
namespace WebBlazor.Client.Services;

public class EventService : IEventService
{
    public event Func<string, Task> BasketUpdated;

    public event Action OrderCreated;

    public async Task OnBasketUpdated(string userId) =>
        await BasketUpdated?.Invoke(userId);

    public void OnOrderCreated() =>
        OrderCreated?.Invoke();
}
