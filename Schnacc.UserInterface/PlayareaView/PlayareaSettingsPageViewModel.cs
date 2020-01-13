using System;
using Schnacc.Domain.Food;
using Schnacc.Domain.Playarea;
using Schnacc.Domain.Snake;
using Schnacc.UserInterface.HomeMenuView;
using Schnacc.UserInterface.Infrastructure.Commands;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;
using Schnacc.UserInterface.LoginView;

namespace Schnacc.UserInterface.PlayareaView
{
    public class PlayareaSettingsPageViewModel : ViewModelBase, INavigatableViewModel
    {
        public INavigationService navigationService { get; set; }

        public RelayCommand GoToMenuView { get; set; }
        public RelayCommand GoToPlayareaView { get; set; }

        public PlayareaSettingsPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.GoToMenuView = new RelayCommand(this.NavigateToMenuView);
            this.GoToPlayareaView = new RelayCommand(this.NavigatePlayarea);
        }

        private void NavigatePlayarea()
        {

            int columns = 8;
            int rows = 10;
            PlayareaSize playareaSize = new PlayareaSize(columns, rows);
            FoodFactory foodFactory = new FoodFactory();
            Position starPosition = new Position(columns / 2, rows / 2);
            Snake snake = new Snake(starPosition);

            Playarea playarea = new Playarea(playareaSize, foodFactory, snake);
            this.navigationService.NavigateTo(new PlayareaViewModel(this.navigationService, playarea));
        }

        private void NavigateToMenuView()
        {
            if (string.IsNullOrEmpty(this.navigationService.SessionToken))
            {
                this.navigationService.NavigateTo(new HomeMenuPageViewModel(this.navigationService));
            }
            else
            {
                this.navigationService.NavigateTo(new LoginSuccessfulPageMenuViewModel(this.navigationService));
            }
        }
    }
}