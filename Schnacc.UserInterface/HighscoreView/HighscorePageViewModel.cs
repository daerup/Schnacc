using Schnacc.Database;
using Schnacc.UserInterface.HomeMenuView;
using Schnacc.UserInterface.Infrastructure.Commands;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;
using Schnacc.UserInterface.LoginView;
using Schnacc.UserInterface.PlayAreaView;

namespace Schnacc.UserInterface.HighScoreView
{
    public class HighscorePageViewModel : ViewModelBase, INavigableViewModel
    {
        public INavigationService NavigationService { get; set; }
        public HighscoreViewModel HighscoreViewModel { get; private set; }
        public RelayCommand GoToPlayareaSettingsView { get; private set; }
        public RelayCommand GoToMenuView { get; private set; }

        public HighscorePageViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
            this.HighscoreViewModel = new HighscoreViewModel(this.NavigationService, new FirebaseDatabase(config: new FirebaseDatabaseConfig { SessionKey = this.NavigationService.SessionToken }));
            this.GoToPlayareaSettingsView = new RelayCommand(this.NavigateToPlayareaSettings);
            this.GoToMenuView = new RelayCommand(this.NavigateToMenu);
        }

        private void NavigateToMenu()
        {
            if(this.NavigationService.AuthorizationApi.IsAnonymous)
            {
                this.NavigationService.NavigateTo(new HomeMenuPageViewModel(this.NavigationService));
            }
            else
            {
                this.NavigationService.NavigateTo(new LoginSuccessfulPageMenuViewModel(this.NavigationService));
            }
        }

        private void NavigateToPlayareaSettings()
        {
            this.NavigationService.NavigateTo(new PlayAreaSettingsPageViewModel(this.NavigationService));
        }
    }
}