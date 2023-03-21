
namespace WebBlazor.Client.Shared;

public partial class LoginDisplay
{
    [Inject]
    private NavigationManager Navigation { get; set; }

    private void BeginSignIn(MouseEventArgs _) =>
        Navigation.NavigateToLogin("authentication/login");

    private void BeginSignOut(MouseEventArgs _) =>
        Navigation.NavigateToLogout("authentication/logout");
}
