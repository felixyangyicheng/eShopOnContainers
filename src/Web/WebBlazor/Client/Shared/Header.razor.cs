
namespace WebBlazor.Client.Shared;

public partial class Header
{
    [Parameter]
    public IEnumerable<HeaderInfo> Model { get; set; }
}
