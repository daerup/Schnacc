using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;

namespace Schnacc.UserInterface.HighscoreView
{
    public class HighscorePageViewModel : ViewModelBase, INavigatableViewModel
    {
        public INavigationService navigationService { get; set; }
    }
}