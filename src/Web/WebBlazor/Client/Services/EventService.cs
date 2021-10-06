using System;
using System.Threading.Tasks;

namespace WebBlazor.Client.Services
{
    public class EventService : IEventService
    {
        public event Func<string, Task> BasketUpdated;

        public event Action OrderCreated;

        public void OnBasketUpdated(string userId) =>
            BasketUpdated?.Invoke(userId);

        public void OnOrderCreated() =>
            OrderCreated?.Invoke();
    }
}
