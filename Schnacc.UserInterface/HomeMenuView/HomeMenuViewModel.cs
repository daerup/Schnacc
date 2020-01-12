using System;
using Schnacc.Domain.Food;
using Schnacc.Domain.Playarea;
using Schnacc.Domain.Snake;
using Schnacc.UserInterface.Infrastructure.Commands;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;
using Schnacc.UserInterface.LoginView;
using Schnacc.UserInterface.PlayareaView;
using Schnacc.UserInterface.RegisterView;

namespace Schnacc.UserInterface.HomeMenuView
{
    public class HomeMenuViewModel : ViewModelBase, INavigatableViewModel
    {
        public RelayCommand GoToPlayareaView { get; }
        public RelayCommand GoToLoginView { get; }
        public RelayCommand GoToRegisterView { get; }

        public HomeMenuViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.GoToPlayareaView = new RelayCommand(this.NavigateToPlayarea);
            this.GoToLoginView = new RelayCommand(this.NavigateToLogin);
            this.GoToRegisterView = new RelayCommand(this.NavigateToRegister);
        }

        private void NavigateToRegister()
        {
            this.navigationService.NavigateTo(new RegisterViewModel(this.navigationService));
        }

        private void NavigateToLogin()
        {
            this.navigationService.NavigateTo(new LoginViewModel(this.navigationService));
        }

        public INavigationService navigationService { get; set; }

        private void NavigateToPlayarea()
        {
            int columns = 8;
            int rows = 10;
            PlayareaSize playareaSize = new PlayareaSize(columns, rows);
            FoodFactory foodFactory = new FoodFactory();
            Position starPosition = new Position(columns/2, rows/2);
            Snake snake = new Snake(starPosition);

            Playarea playarea = new Playarea(playareaSize, foodFactory, snake);
            this.navigationService.NavigateTo(new PlayareaViewModel(this.navigationService, playarea));
        }
    };
}