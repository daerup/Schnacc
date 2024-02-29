using Newtonsoft.Json;

namespace Schnacc.Database
{
    public class Highscore
    {
        public Highscore()
        {
        }

        public Highscore(string username, double score)
        {
            this.Username = username;
            this.Score = score;
        }

        [JsonProperty("Score")]
        public double Score { get; set; }

        [JsonProperty("Username")]
        public string Username { get; set; }
    }
}