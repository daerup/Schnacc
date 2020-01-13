using System;
using System.Windows;
using Schnacc.Domain.Food;
using Schnacc.Domain.Playarea;
using Schnacc.Domain.Snake;
using Schnacc.UserInterface.HighscoreView;
using Schnacc.UserInterface.Infrastructure.Commands;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;
using Schnacc.UserInterface.LoginView;
using Schnacc.UserInterface.PlayareaView;
using Schnacc.UserInterface.RegisterView;

namespace Schnacc.UserInterface.HomeMenuView
{
    public class HomeMenuPageViewModel : ViewModelBase, INavigatableViewModel
    {
        public RelayCommand GoToPlayareaView { get; }
        public RelayCommand GoToHighscoresView { get; }
        public RelayCommand GoToLoginView { get; }
        public RelayCommand GoToRegisterView { get; }

        public HomeMenuPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.GoToPlayareaView = new RelayCommand(this.NavigateToPlayarea);
            this.GoToHighscoresView = new RelayCommand(this.NavigateToHighscores);
            this.GoToLoginView = new RelayCommand(this.NavigateToLogin);
            this.GoToRegisterView = new RelayCommand(this.NavigateToRegister);
        }

        public INavigationService navigationService { get; set; }

        private void NavigateToHighscores()
        {
            if (string.IsNullOrEmpty(this.navigationService.SessionToken))
            {
                MessageBox.Show(
                    "Highscores is a premium feature and can only be used by logged in users",
                    "Premium Feature", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Warning);
                this.NavigateToLogin();
                return;
            }
            this.navigationService.NavigateTo(new HighscorePageViewModel());
        }

        private void NavigateToRegister()
        {
            this.navigationService.NavigateTo(new RegisterPageViewModel(this.navigationService));
        }

        private void NavigateToLogin()
        {
            this.navigationService.NavigateTo(new LoginPageViewModel(this.navigationService));
        }

        private void NavigateToPlayarea()
        {
            int columns = 8;
            int rows = 10;
            PlayareaSize playareaSize = new PlayareaSize(columns, rows);
            FoodFactory foodFactory = new FoodFactory();
            Position starPosition = new Position(columns/2, rows/2);
            Snake snake = new Snake(starPosition);

            Playarea playarea = new Playarea(playareaSize, foodFactory, snake);
            this.navigationService.NavigateTo(new PlayareaPageViewModel(this.navigationService, playarea));
        }
    };
}