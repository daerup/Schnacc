using Schnacc.Authorization;

namespace Schnacc.UserInterface.Infrastructure.Navigation
{
    public class NavigationService : INavigationService
    {
        private AuthorizationApi _authApi;

        public delegate void NavigateHandler(object o, NavigationEventArgs args);

        public event NavigateHandler OnNavigation;

        public void NavigateTo(INavigableViewModel viewModel)
        {
            this.OnNavigation?.Invoke(this, new NavigationEventArgs(viewModel));
        }

        public string Username => this._authApi.Username;

        public string SessionToken { get; set; }
        public bool EmailIsVerified => this._authApi.EmailIsVerified;
        public void SetAuthApi(AuthorizationApi api)
        {
            this._authApi = api;
        }
    }
}