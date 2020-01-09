namespace Schnacc.Database
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Transactions;

    using Firebase.Database;
    using Firebase.Database.Query;

    public class Database
    {
        private FirebaseClient firebaseClient;

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
            List<Highscore> highscores = new List<Highscore>();
            IReadOnlyCollection<FirebaseObject<Highscore>> firebaseHighscores = this.firebaseClient.Child("Highscores")
                .OrderByKey()
                .OnceAsync<Highscore>().Result;

            foreach (FirebaseObject<Highscore> highscore in firebaseHighscores)
            {
                highscores.Add(highscore.Object);
            }

            return highscores;
        }
    }
}
