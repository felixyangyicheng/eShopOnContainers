using System.Collections.ObjectModel;

namespace WebBlazor.Client.Services.ModelDTOs;

public record CampaignDTO
{
    public int PageIndex { get; init; }

    public int PageSize { get; init; }

    public int Count { get; init; }

    public Collection<CampaignItemDTO> Data { get; init; }
}
