﻿using Schnacc.Domain.Food;
using Schnacc.Domain.Playarea;
using Schnacc.Domain.Snake;
using Schnacc.UserInterface.HighscoreView;
using Schnacc.UserInterface.Infrastructure.Commands;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;
using Schnacc.UserInterface.PlayareaView;

namespace Schnacc.UserInterface.LoginView
{
    public class LoginSuccessfulViewModel : ViewModelBase, INavigatableViewModel
    {
        public RelayCommand GoToHighscoreView { get; }

        public RelayCommand GoToPlayareaView { get; }

        public LoginSuccessfulViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.GoToPlayareaView = new RelayCommand(this.NavigateToPlayarea);
            this.GoToHighscoreView = new RelayCommand(this.NavigateToHighscore);
        }

        private void NavigateToHighscore()
        {
            this.navigationService.NavigateTo(new HighscoreViewModel(this.navigationService));
        }

        public INavigationService navigationService { get; set; }

        public bool WarningIsVisible => !this.navigationService.EmailIsVerified;

        private void NavigateToPlayarea()
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
    }
}