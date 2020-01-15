using System.Security.Cryptography.Pkcs;
using Schnacc.Authorization;
using Schnacc.UserInterface.Infrastructure.ViewModels;

namespace Schnacc.UserInterface.Infrastructure.Navigation
{
    public class NavigationService : INavigationService
    {
        private AuthorizationApi authApi;
        private IViewModel viewModelToNavigateTo;

        public delegate void NavigateHandler(object o, NavigationEventArgs args);

        public event NavigateHandler OnNavigation;

        public void NavigateTo(INavigatableViewModel viewModel )
        {
            this.viewModelToNavigateTo = viewModel;
            this.OnNavigation(this, new NavigationEventArgs(this.viewModelToNavigateTo));
        }

        public string Username => this.authApi.Username;

        public string SessionToken { get; set; }
        public bool EmailIsVerified => this.authApi.EmailIsVerified;
        public void SetAuthApi(AuthorizationApi api)
        {
            this.authApi = api;
        }
    }
}