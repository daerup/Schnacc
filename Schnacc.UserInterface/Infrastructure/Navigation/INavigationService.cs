using Schnacc.Authorization;

namespace Schnacc.UserInterface.Infrastructure.Navigation
{
    using ViewModels;
    public interface INavigationService
    {
        void NavigateTo(INavigatableViewModel viewModel);
        string Username { get; }
        string SessionToken { get; set; }
        bool EmailIsVerified { get; }
        void SetAuthApi(AuthorizationApi api);
    }
}
