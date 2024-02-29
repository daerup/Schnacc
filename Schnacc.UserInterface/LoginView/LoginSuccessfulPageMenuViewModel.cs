using Schnacc.UserInterface.HighscoreView;
using Schnacc.UserInterface.Infrastructure.Commands;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;
using Schnacc.UserInterface.PlayareaView;

namespace Schnacc.UserInterface.LoginView
{
    public class LoginSuccessfulPageMenuViewModel : ViewModelBase, INavigatableViewModel
    {
        private bool mailSent;
        public RelayCommand GoToHighscoreViewCommand { get; }

        public RelayCommand GoToPlayareaViewCommand { get; }
        
        public LoginSuccessfulPageMenuViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
            this.GoToPlayareaViewCommand = new RelayCommand(this.NavigateToPlayareaSettings);
            this.GoToHighscoreViewCommand = new RelayCommand(this.NavigateToHighscore);
        }

        private void NavigateToHighscore()
        {
            this.NavigationService.NavigateTo(new HighscorePageViewModel(this.NavigationService));
            this.mailSent = true;
            this.MessageContent = "We sent an email to you";
        }

        public INavigationService NavigationService { get; set; }

        public bool MessageIsVisible => !string.IsNullOrEmpty(this.MessageContent);

        public string MessageContent { get; private set; } =
            "Warning: You're E-Mail is not verified. You can't upload you're highscore unless your E-Mail is verified";


        private void NavigateToPlayareaSettings()
        {
            this.NavigationService.NavigateTo(new PlayareaSettingsPageViewModel(this.NavigationService));
        }
    }
}