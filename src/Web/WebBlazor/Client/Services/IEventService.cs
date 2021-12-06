using System;
using System.Threading.Tasks;

namespace WebBlazor.Client.Services;

public interface IEventService
{
    event Func<string, Task> BasketUpdated;

    event Action OrderCreated;

    Task OnBasketUpdated(string userId);

    void OnOrderCreated();
}
