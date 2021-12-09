
namespace WebBlazor.Client.Services;

public interface ICatalogService
{
    Task<CatalogDTO> GetCatalogItems(int page, int take, int? brand, int? type);

    Task<IEnumerable<BrandDTO>> GetBrands();

    Task<IEnumerable<TypeDTO>> GetTypes();
}
