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
            this.navigationService = navigationService;
            this.GoToPlayareaView = new RelayCommand(this.NavigateToPlayareaSettings);
            this.GoToHighscoreView = new RelayCommand(this.NavigateToHighscore);
        }

        private void NavigateToHighscore()
        {
            this.navigationService.NavigateTo(new HighscorePageViewModel(this.navigationService));
        }

        public INavigationService navigationService { get; set; }

        public bool WarningIsVisible => !this.navigationService.EmailIsVerified;

        private void NavigateToPlayareaSettings()
        {
            this.navigationService.NavigateTo(new PlayareaSettingsPageViewModel(this.navigationService));
        }
    }
}