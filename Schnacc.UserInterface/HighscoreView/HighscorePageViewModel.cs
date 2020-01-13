using System.CodeDom;
using Schnacc.UserInterface.Infrastructure.Commands;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;
using Schnacc.UserInterface.LoginView;
using Schnacc.UserInterface.PlayareaView;

namespace Schnacc.UserInterface.HighscoreView
{
    public class HighscorePageViewModel : ViewModelBase, INavigatableViewModel
    {
        public INavigationService navigationService { get; set; }
        public HighscoreViewModel HighscoreViewModel { get; private set; }
        public RelayCommand GoToPlayareaSettingsView { get; private set; }
        public RelayCommand GoToLoginSuccessfulMenusView { get; private set; }

        public HighscorePageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.HighscoreViewModel = new HighscoreViewModel(this.navigationService, new Database.Database(this.navigationService.SessionToken));
            this.GoToPlayareaSettingsView = new RelayCommand(this.NavigateToPlayareaSettings);
            this.GoToLoginSuccessfulMenusView = new RelayCommand(this.NavigateToLoginSuccessfulMenu);
        }

        private void NavigateToLoginSuccessfulMenu()
        {
            this.navigationService.NavigateTo(new LoginSuccessfulPageMenuViewModel(this.navigationService));
        }

        private void NavigateToPlayareaSettings()
        {
            this.navigationService.NavigateTo(new PlayareaSettingsPageViewModel(this.navigationService));
        }
    }
}