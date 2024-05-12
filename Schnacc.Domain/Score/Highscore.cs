namespace Schnacc.Domain.Score
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

        public double Score { get; set; }

        public string Username { get; set; }
    }
}