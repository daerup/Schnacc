using System;
using Schnacc.Domain.Food;
using Schnacc.Domain.Playarea;
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

        public RelayCommand GoToMenuView { get; }
        public RelayCommand GoToPlayareaView { get; }

        public int DifficultyLevel { get; set; } = 9;

        public string NumberOfRows { get; set; } = "10";
        public string NumberOfColumns { get; set; } = "10";

        public PlayareaSettingsPageViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
            this.GoToMenuView = new RelayCommand(this.NavigateToMenuView);
            this.GoToPlayareaView = new RelayCommand(this.NavigatePlayarea);
        }

        private void NavigatePlayarea()
        {
            PlayareaSize playareaSize = new PlayareaSize(Convert.ToInt32(this.NumberOfRows), Convert.ToInt32(this.NumberOfColumns));
            FoodFactory foodFactory = new FoodFactory();

            Playarea playarea = new Playarea(playareaSize, foodFactory);
            this.NavigationService.NavigateTo(new PlayareaViewModel(this.NavigationService, playarea, this.DifficultyLevel));
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