namespace Schnacc.Database
{
    using Newtonsoft.Json;

    public class Highscore
    {
        public Highscore()
        {
        }

        public Highscore(string username, double score)
        {
            username = this.Username;
            score = this.Score;
        }

        [JsonProperty("Score")]
        public double Score { get; set; }

        [JsonProperty("Username")]
        public string Username { get; set; }
    }
}