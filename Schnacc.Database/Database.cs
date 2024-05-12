using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Database.Streaming;

namespace Schnacc.Database
{
    public class Database
    {
        private readonly FirebaseDatabaseConfig _config;
        private readonly FirebaseClient _firebaseClient;
        
        public Database(FirebaseDatabaseConfig config)
        {
            this._config = config;
            this._firebaseClient = new FirebaseClient(
                this._config.BaseUrl,
                new FirebaseOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(this._config.ApiKey)
                    });
        }

        public IEnumerable<Highscore> GetHighscores()
        {
            return this._firebaseClient.Child(this._config.DatabaseChild)
                                       .OrderByKey()
                                       .OnceAsync<Highscore>().Result
                                       .Select(highscore => highscore.Object);
        }

        public IObservable<IReadOnlyCollection<Highscore>> GetObservableHighscores()
        {
            return this._firebaseClient
                       .Child(this._config.DatabaseChild)
                       .AsObservable<Highscore>()
                       .Scan(new List<FirebaseEvent<Highscore>>(), (list, change) =>
                       {
                           switch(change.EventType)
                           {
                               case FirebaseEventType.InsertOrUpdate:
                                   int index = list.FindIndex(h => h.Key == change.Key);
                                   
                                   if(index == -1)
                                   {
                                       list.Add(change);
                                   }
                                   else
                                   {
                                       list[index] = change;
                                   } 
                                   break;
                               case FirebaseEventType.Delete:
                                   list.RemoveAll(h => h.Key == change.Key);
                                   break;
                               default:
                                   throw new ArgumentOutOfRangeException();
                           }
                           return list;
                       })
                       .Select(list => list.Select(h => h.Object).ToList())
                       .Select(list => (IReadOnlyCollection<Highscore>)list.AsReadOnly());
        }

        public async Task WriteHighScore(Highscore highScore)
        {
            await this._firebaseClient.Child(this._config.DatabaseChild).PostAsync(highScore);
        }
    }
}
