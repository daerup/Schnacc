using Schnacc.UserInterface.HighScoreView;
using Schnacc.UserInterface.Infrastructure.Commands;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;
using Schnacc.UserInterface.PlayAreaView;

namespace Schnacc.UserInterface.LoginView
{
    public class LoginSuccessfulPageMenuViewModel : ViewModelBase, INavigableViewModel
    {
        public RelayCommand GoToHighScoreViewCommand { get; }

        public RelayCommand GoToPlayAreaViewCommand { get; }
        
        public LoginSuccessfulPageMenuViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
            this.GoToPlayAreaViewCommand = new RelayCommand(this.NavigateToPlayAreaSettings);
            this.GoToHighScoreViewCommand = new RelayCommand(this.NavigateToHighScore);
        }

        private void NavigateToHighScore()
        {
            this.NavigationService.NavigateTo(new HighscorePageViewModel(this.NavigationService));
            this.MessageContent = "We sent an email to you";
        }

        public INavigationService NavigationService { get; set; }

        public bool MessageIsVisible => !string.IsNullOrEmpty(this.MessageContent);

        public string MessageContent { get; private set; } =
            "Warning: You're E-Mail is not verified. You can't upload your highscore unless your E-Mail is verified";


        private void NavigateToPlayAreaSettings() => 
            this.NavigationService.NavigateTo(new PlayAreaSettingsPageViewModel(this.NavigationService));
    }
}