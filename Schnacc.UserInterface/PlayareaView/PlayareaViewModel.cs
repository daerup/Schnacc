using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;

namespace Schnacc.UserInterface.PlayareaView
{
    public class PlayareaViewModel : ViewModelBase, INavigatableViewModel
    {
        public PlayareaViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public INavigationService navigationService { get; set; }
    }
}