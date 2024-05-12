using Schnacc.UserInterface.HighScoreView;
using Schnacc.UserInterface.HomeMenuView;
using Schnacc.UserInterface.Infrastructure.Commands;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;
using Schnacc.UserInterface.PlayAreaView;
using System.Threading.Tasks;

namespace Schnacc.UserInterface.LoginView
{
    public class LoginSuccessfulPageMenuViewModel : ViewModelBase, INavigableViewModel
    {
        public RelayCommand GoToHighScoreViewCommand { get; }

        public RelayCommand GoToPlayAreaViewCommand { get; }
        public AsyncRelayCommand<object> GoToLoggedOutViewCommand { get; }

        public LoginSuccessfulPageMenuViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
            this.GoToPlayAreaViewCommand = new RelayCommand(this.NavigateToPlayAreaSettings);
            this.MessageContent = this.NavigationService.AuthorizationApi.EmailIsVerified 
                ? string.Empty 
                : "Warning: You're E-Mail is not verified. You can't upload your highscore unless your E-Mail is verified";
            this.GoToHighScoreViewCommand = new RelayCommand(this.NavigateToHighScore);
            this.GoToLoggedOutViewCommand = new AsyncRelayCommand<object>(this.NavigateToLoggedOutMenu);
        }

        private void NavigateToHighScore()
        {
            this.NavigationService.NavigateTo(new HighscorePageViewModel(this.NavigationService));
            this.MessageContent = "We sent an email to you";
        }

        public INavigationService NavigationService { get; set; }

        public bool MessageIsVisible => !string.IsNullOrEmpty(this.MessageContent);

        public string MessageContent { get; private set; }
        
        private void NavigateToPlayAreaSettings() => 
            this.NavigationService.NavigateTo(new PlayAreaSettingsPageViewModel(this.NavigationService));

        private async Task NavigateToLoggedOutMenu(object _)
        {
            this.NavigationService.AuthorizationApi.SignOut();
            this.NavigationService.SessionToken = await this.NavigationService.AuthorizationApi.SignInAnonymous();
            this.NavigationService.NavigateTo(new HomeMenuPageViewModel(this.NavigationService));
        }
    }
}