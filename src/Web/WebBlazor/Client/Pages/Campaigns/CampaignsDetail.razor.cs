
namespace WebBlazor.Client.Pages.Campaigns;

[Authorize]
public partial class CampaignsDetail
{
    private CampaignItemDTO campaign = new();
    private readonly List<HeaderInfo> header = new()
    {
        new() { Url = "catalog", Text = "Back to catalog" },
        new() { Url = "campaigns", Text = "Back to campaigns" }
    };

    [Inject]
    private ICampaignService CampaignService { get; set; }

    [Parameter]
    public int Id { get; set; }

    protected override async Task OnInitializedAsync() =>
        await GetCampaign();

    private async Task GetCampaign() =>
        campaign = await CampaignService.GetCampaignById(Id);
}
