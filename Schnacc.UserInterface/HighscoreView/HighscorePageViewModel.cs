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
        public RelayCommand GoToLoginSuccessfulMenusView { get; private set; }

        public HighscorePageViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
            this.HighscoreViewModel = new HighscoreViewModel(this.NavigationService, new Database.Database(this.NavigationService.SessionToken));
            this.GoToPlayareaSettingsView = new RelayCommand(this.NavigateToPlayareaSettings);
            this.GoToLoginSuccessfulMenusView = new RelayCommand(this.NavigateToLoginSuccessfulMenu);
        }

        private void NavigateToLoginSuccessfulMenu()
        {
            this.NavigationService.NavigateTo(new LoginSuccessfulPageMenuViewModel(this.NavigationService));
        }

        private void NavigateToPlayareaSettings()
        {
            this.NavigationService.NavigateTo(new PlayAreaSettingsPageViewModel(this.NavigationService));
        }
    }
}