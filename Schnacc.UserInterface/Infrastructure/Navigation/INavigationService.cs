using Schnacc.Authorization;

namespace Schnacc.UserInterface.Infrastructure.Navigation
{
    using ViewModels;
    public interface INavigationService
    {
        public void NavigateTo(INavigatableViewModel viewModel);
        public string Username { get; }
        public string SessionToken { get; set; }
        public bool EmailIsVerified { get; }
        public void SetAuthApi(AuthorizationApi api);
    }
}
