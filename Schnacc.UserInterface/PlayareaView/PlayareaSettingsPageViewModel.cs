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
        public INavigationService NavigationService { get; set; }

        public RelayCommand GoToMenuView { get; set; }
        public RelayCommand GoToPlayareaView { get; set; }

        public PlayareaSettingsPageViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
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

            Playarea playarea = new Playarea(playareaSize, foodFactory);
            this.NavigationService.NavigateTo(new PlayareaViewModel(this.NavigationService, playarea));
        }

        private void NavigateToMenuView()
        {
            if (string.IsNullOrEmpty(this.NavigationService.SessionToken))
            {
                this.NavigationService.NavigateTo(new HomeMenuPageViewModel(this.NavigationService));
            }
            else
            {
                this.NavigationService.NavigateTo(new LoginSuccessfulPageMenuViewModel(this.NavigationService));
            }
        }
    }
}