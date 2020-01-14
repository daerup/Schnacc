using Schnacc.Domain.Food;
using Schnacc.Domain.Playarea;
using Schnacc.Domain.Snake;
using Schnacc.UserInterface.HighscoreView;
using Schnacc.UserInterface.Infrastructure.Commands;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;
using Schnacc.UserInterface.PlayareaView;

namespace Schnacc.UserInterface.LoginView
{
    public class LoginSuccessfulPageMenuViewModel : ViewModelBase, INavigatableViewModel
    {
        public RelayCommand GoToHighscoreView { get; }

        public RelayCommand GoToPlayareaView { get; }

        public LoginSuccessfulPageMenuViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
            this.GoToPlayareaView = new RelayCommand(this.NavigateToPlayareaSettings);
            this.GoToHighscoreView = new RelayCommand(this.NavigateToHighscore);
        }

        private void NavigateToHighscore()
        {
            this.NavigationService.NavigateTo(new HighscorePageViewModel(this.NavigationService));
        }

        public INavigationService NavigationService { get; set; }

        public bool WarningIsVisible => !this.NavigationService.EmailIsVerified;

        private void NavigateToPlayareaSettings()
        {
            this.NavigationService.NavigateTo(new PlayareaSettingsPageViewModel(this.NavigationService));
        }
    }
}