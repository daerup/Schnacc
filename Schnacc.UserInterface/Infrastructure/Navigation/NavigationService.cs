using System.Security.Cryptography.Pkcs;
using Schnacc.UserInterface.Infrastructure.ViewModels;

namespace Schnacc.UserInterface.Infrastructure.Navigation
{
    public class NavigationService : INavigationService
    {
        private IViewModel viewModelToNavigateTo;
        public delegate void NavigateHandler(object o, NavigationEventArgs args);

        public event NavigateHandler OnNavigation;

        public void NavigateTo(INavigatableViewModel viewModel )
        {
            this.viewModelToNavigateTo = viewModel;
            this.OnNavigation(this, new NavigationEventArgs(this.viewModelToNavigateTo));
        }

        public string SessionToken { get; set; }
        public bool EmailIsVerified { get; set; }
    }
}
