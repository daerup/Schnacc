using System.Reactive.Linq;
using Firebase.Database.Streaming;

namespace Schnacc.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Transactions;

    using Firebase.Database;
    using Firebase.Database.Query;

    public class Database
    {
        private readonly FirebaseClient firebaseClient;

        public Database(string accessToken)
        {
            this.firebaseClient = new FirebaseClient(
                DatabaseConfig.BaseUrl,
                new FirebaseOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(accessToken)
                    });
        }

        public List<Highscore> GetHighscores()
        {
            IReadOnlyCollection<FirebaseObject<Highscore>> firebaseHighscores = this.firebaseClient.Child(DatabaseConfig.DatabaseChild)
                .OrderByKey()
                .OnceAsync<Highscore>().Result;

            return firebaseHighscores.Select(highscore => highscore.Object).ToList();
        }

        public IObservable<FirebaseEvent<Highscore>> GetObservableHighscores()
        {
            IObservable<FirebaseEvent<Highscore>> firebaseHighscores = this.firebaseClient
                .Child(DatabaseConfig.DatabaseChild)
                .AsObservable<Highscore>();
            return firebaseHighscores;
        }

        public async void WriteHighscore(Highscore highscore)
        {
            FirebaseObject<Highscore> firebaseObject = await this.firebaseClient.Child(DatabaseConfig.DatabaseChild).PostAsync(highscore, false);
        }
    }
}
