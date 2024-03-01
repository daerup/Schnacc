using System;
using System.Collections.Generic;
using Schnacc.Database;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;

namespace Schnacc.UserInterface.HighScoreView
{
    public class HighscoreViewModel : ViewModelBase 
    { 

        private Database.Database db;
        public HighscoreViewModel(INavigationService navigationService, Database.Database db)
        {
            this.NavigationService = navigationService;

            if (string.IsNullOrEmpty(this.NavigationService.SessionToken))
            {
                return;
            }

            this.db = db;
            this.Highscores = this.db.GetHighscores();
            this.db.GetObservableHighscores().Subscribe(s => this.UpdateHighscores());
        }

        public INavigationService NavigationService { get; }

        public List<Highscore> Highscores { get; private set;}

        private void UpdateHighscores()
        {
            this.Highscores = this.db.GetHighscores();
            this.OnPropertyChanged(nameof(this.Highscores));
        }

    }
}
