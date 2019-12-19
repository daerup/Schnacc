namespace Schnacc.Database
{
    using Newtonsoft.Json;

    public class Highscore
    {
            [JsonProperty("Score")]
            public double Score { get; set; }

            [JsonProperty("Username")]
            public string Username { get; set; }
    }
}