namespace Schnacc.Database
{
    public class FirebaseDatabaseConfig
    {
        public string BaseUrl => "https://schnacc-swat-default-rtdb.firebaseio.com/";
        public string DatabaseChild => "Highscores";
        public string SessionKey{ get; set; }
    }
}
