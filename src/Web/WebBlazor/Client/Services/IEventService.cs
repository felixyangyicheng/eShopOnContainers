using System;
using System.Threading.Tasks;

namespace WebBlazor.Client.Services
{
    public interface IEventService
    {
        event Func<string, Task> BasketUpdated;

        event Action OrderCreated;

        void OnBasketUpdated(string userId);

        void OnOrderCreated();
    }
}
