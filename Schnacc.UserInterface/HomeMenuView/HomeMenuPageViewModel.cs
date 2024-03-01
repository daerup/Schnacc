using System.Windows;
using Schnacc.UserInterface.HighScoreView;
using Schnacc.UserInterface.Infrastructure.Commands;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;
using Schnacc.UserInterface.LoginView;
using Schnacc.UserInterface.PlayAreaView;
using Schnacc.UserInterface.RegisterView;

namespace Schnacc.UserInterface.HomeMenuView
{
    public class HomeMenuPageViewModel : ViewModelBase, INavigatableViewModel
    {
        public RelayCommand GoToPlayareaSettingsView { get; }
        public RelayCommand GoToHighscoresView { get; }
        public RelayCommand GoToLoginView { get; }
        public RelayCommand GoToRegisterView { get; }

        public HomeMenuPageViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
            this.GoToPlayareaSettingsView = new RelayCommand(this.NavigateToPlayareaSettings);
            this.GoToHighscoresView = new RelayCommand(this.NavigateToHighscores);
            this.GoToLoginView = new RelayCommand(this.NavigateToLogin);
            this.GoToRegisterView = new RelayCommand(this.NavigateToRegister);
        }

        public INavigationService NavigationService { get; set; }

        private void NavigateToHighscores()
        {
            if (string.IsNullOrEmpty(this.NavigationService.SessionToken))
            {
                MessageBox.Show(
                    "Highscores is a premium feature and can only be used by logged in users",
                    "Premium Feature", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Warning);
                this.NavigateToLogin();
                return;
            }
            this.NavigationService.NavigateTo(new HighscorePageViewModel(this.NavigationService));
        }

        private void NavigateToRegister()
        {
            this.NavigationService.NavigateTo(new RegisterPageViewModel(this.NavigationService));
        }

        private void NavigateToLogin()
        {
            this.NavigationService.NavigateTo(new LoginPageViewModel(this.NavigationService));
        }

        private void NavigateToPlayareaSettings()
        {
            this.NavigationService.NavigateTo(new PlayAreaSettingsPageViewModel(this.NavigationService));
        }
    }
}