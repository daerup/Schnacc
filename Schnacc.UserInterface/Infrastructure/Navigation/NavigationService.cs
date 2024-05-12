using Schnacc.Authorization;

namespace Schnacc.UserInterface.Infrastructure.Navigation
{
    public class NavigationService : INavigationService
    {
        private AuthorizationApi _authApi;

        public NavigationService(AuthorizationApi authApi)
        {
            this._authApi = authApi;
        }

        public delegate void NavigateHandler(object o, NavigationEventArgs args);

        public event NavigateHandler OnNavigation;

        public void NavigateTo(INavigableViewModel viewModel)
        {
            this.OnNavigation?.Invoke(this, new NavigationEventArgs(viewModel));
        }

        public AuthorizationApi AuthorizationApi => this._authApi;
        public string Username => this._authApi.Username;
        public string SessionToken { get; set; }
        public bool EmailIsVerified => this._authApi.EmailIsVerified;
    }
}