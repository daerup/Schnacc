using System;
using System.Collections.Generic;
using Firebase.Database.Streaming;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;
using Schnacc.Database;

namespace Schnacc.UserInterface.HighscoreView
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
            this.db.GetObservableHighscores().Subscribe(s => this.UpdateHighscores(s)); ;
        }

        public INavigationService NavigationService { get; set; }

        public List<Highscore> Highscores { get; private set;}

        private void UpdateHighscores(FirebaseEvent<Highscore> firebaseEvent)
        {
            this.Highscores = this.db.GetHighscores();
        }

    }
}
