using Schnacc.Authorization;

namespace Schnacc.UserInterface.Infrastructure.Navigation
{
    public class NavigationService : INavigationService
    {
        public NavigationService(IAuthorizationApi authApi)
        {
            this.AuthorizationApi = authApi;
        }

        public delegate void NavigateHandler(object o, NavigationEventArgs args);

        public event NavigateHandler OnNavigation;

        public void NavigateTo(INavigableViewModel viewModel)
        {
            this.OnNavigation?.Invoke(this, new NavigationEventArgs(viewModel));
        }

        public IAuthorizationApi AuthorizationApi { get; }
        public string SessionToken { get; set; }
    }
}