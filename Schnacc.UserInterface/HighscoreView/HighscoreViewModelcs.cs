using System;
using System.Collections.Generic;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;
using Schnacc.Database;

namespace Schnacc.UserInterface.HighscoreView
{
    class HighscoreViewModel : ViewModelBase, INavigatableViewModel
    {
        private Database.Database db;
        public HighscoreViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            if (string.IsNullOrEmpty(this.navigationService.SessionToken))
            {
                return;
            }
            this.db = new Database.Database(this.navigationService.SessionToken);
            List<Highscore> highscores = this.db.GetHighscores();
            this.Highscores = this.db.GetObservableHighscores();
        }


        public INavigationService navigationService { get; set; }

        public IObservable<object> Highscores { get; }
    }
}
