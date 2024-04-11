using Schnacc.Authorization;

namespace Schnacc.UserInterface.Infrastructure.Navigation
{
    public interface INavigationService
    {
        void NavigateTo(INavigableViewModel viewModel);
        string Username { get; }
        string SessionToken { get; set; }
        bool EmailIsVerified { get; }
        void SetAuthApi(AuthorizationApi api);
    }
}
