using System.Security.Cryptography.Pkcs;
using Schnacc.Authorization;
using Schnacc.UserInterface.Infrastructure.ViewModels;

namespace Schnacc.UserInterface.Infrastructure.Navigation
{
    public class NavigationService : INavigationService
    {
        private IViewModel viewModelToNavigateTo;
        private AuthorizationApi authApi;

        public delegate void NavigateHandler(object o, NavigationEventArgs args);

        public event NavigateHandler OnNavigation;

        public void NavigateTo(INavigatableViewModel viewModel )
        {
            this.viewModelToNavigateTo = viewModel;
            this.OnNavigation(this, new NavigationEventArgs(this.viewModelToNavigateTo));
        }

        public string Username => this.authApi.Username;

        public string SessionToken => this.authApi.AccessToken;
        public bool EmailIsVerified => this.authApi.EmailIsVerified;
        public void SetAuthApi(AuthorizationApi api)
        {
            this.authApi = api;
        }
    }
}
