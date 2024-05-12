using Schnacc.Authorization;

namespace Schnacc.UserInterface.Infrastructure.Navigation
{
    public interface INavigationService
    {
        void NavigateTo(INavigableViewModel viewModel);
        AuthorizationApi AuthorizationApi { get; }
        string Username { get; }
        string SessionToken { get; set; }
        bool EmailIsVerified { get; }
    }
}
