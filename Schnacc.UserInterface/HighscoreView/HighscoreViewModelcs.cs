using System;
using System.Collections.Generic;
using System.Linq;
using Schnacc.Database;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;

namespace Schnacc.UserInterface.HighScoreView
{
    public class HighscoreViewModel : ViewModelBase 
    { 
        private readonly FirebaseDatabase _db;
        private INavigationService NavigationService { get; }

        public List<Highscore> Highscores { get; private set;}
        public HighscoreViewModel(INavigationService navigationService, FirebaseDatabase db)
        {
            this.NavigationService = navigationService;

            if (string.IsNullOrEmpty(this.NavigationService.SessionToken))
            {
                return;
            }

            this._db = db;
            this.Highscores = this._db.GetHighscores().ToList();
            this._db.GetObservableHighscores().Subscribe(this.UpdateHighscores);
        }

        private void UpdateHighscores(IReadOnlyCollection<Highscore> highscores)
        {
            this.Highscores = highscores.ToList();
            this.OnPropertyChanged(nameof(this.Highscores));
        }
    }
}
