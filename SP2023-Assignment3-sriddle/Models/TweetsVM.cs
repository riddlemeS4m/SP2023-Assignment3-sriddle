namespace SP2023_Assignment3_sriddle.Models
{
    public class TweetsVM
    {
        public string Name { get; set; }
        public double Average { get; set; }
        public List<AnalyzeTweet> Tweets { get; set; }
    }
}
