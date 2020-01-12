using System;
using System.Collections.Generic;
using Firebase.Database.Streaming;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;
using Schnacc.Database;

namespace Schnacc.UserInterface.HighscoreView
{
    public class HighscoreViewModel : ViewModelBase, INavigatableViewModel
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
            this.Highscores = this.db.GetHighscores();
            this.db.GetObservableHighscores().Subscribe(s => this.UpdateHighscores(s)); ;
            
        }

        private void UpdateHighscores(FirebaseEvent<Highscore> firebaseEvent)
        {
            this.Highscores = this.db.GetHighscores();
        }


        public INavigationService navigationService { get; set; }

        public List<Highscore> Highscores { get; private set;}
    }
}
