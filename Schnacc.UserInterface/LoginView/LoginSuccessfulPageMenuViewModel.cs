using Schnacc.UserInterface.HighScoreView;
using Schnacc.UserInterface.HomeMenuView;
using Schnacc.UserInterface.Infrastructure.Commands;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;
using Schnacc.UserInterface.PlayAreaView;
using System;
using System.Runtime.CompilerServices;

namespace Schnacc.UserInterface.LoginView
{
    public class LoginSuccessfulPageMenuViewModel : ViewModelBase, INavigableViewModel
    {
        public RelayCommand GoToHighScoreViewCommand { get; }

        public RelayCommand GoToPlayAreaViewCommand { get; }
        public RelayCommand GoToLoggedoutViewCommand { get; }

        public LoginSuccessfulPageMenuViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
            this.GoToPlayAreaViewCommand = new RelayCommand(this.NavigateToPlayAreaSettings);
            this.MessageContent = this.NavigationService.AuthorizationApi.EmailIsVerified 
                ? string.Empty 
                : "Warning: You're E-Mail is not verified. You can't upload your highscore unless your E-Mail is verified";
            this.GoToHighScoreViewCommand = new RelayCommand(this.NavigateToHighScore);
            this.GoToLoggedoutViewCommand = new RelayCommand(this.NavigateToLoggedOutMenu);
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

        private void NavigateToLoggedOutMenu()
        {
            this.NavigationService.AuthorizationApi.SignOut();
            this.NavigationService.SessionToken = null;
            this.NavigationService.NavigateTo(new HomeMenuPageViewModel(this.NavigationService));
        }
    }
}