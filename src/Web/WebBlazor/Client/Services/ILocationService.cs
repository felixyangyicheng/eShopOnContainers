
namespace WebBlazor.Client.Services;

public interface ILocationService
{
    Task CreateOrUpdateUserLocation(LocationDTO location);
}
