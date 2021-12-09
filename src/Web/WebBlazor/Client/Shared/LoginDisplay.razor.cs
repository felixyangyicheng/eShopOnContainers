
namespace WebBlazor.Client.Shared;

public partial class LoginDisplay
{
    [Inject]
    private NavigationManager Navigation { get; set; }

    [Inject]
    private SignOutSessionStateManager SignOutManager { get; set; }

    private void BeginSignIn(MouseEventArgs _) =>
        Navigation.NavigateTo("authentication/login");

    private async Task BeginSignOut(MouseEventArgs _)
    {
        await SignOutManager.SetSignOutState();
        Navigation.NavigateTo("authentication/logout");
    }
}
