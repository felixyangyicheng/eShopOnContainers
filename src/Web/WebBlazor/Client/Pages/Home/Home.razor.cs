using Microsoft.AspNetCore.Components;

namespace WebBlazor.Client.Pages.Home
{
    [Route("/")]
    public partial class Home : ComponentBase
    {
        [Inject]
        private NavigationManager Navigation { get; set; }

        protected override void OnInitialized() =>
            Navigation.NavigateTo("/catalog");
    }
}
