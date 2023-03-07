namespace SP2023_Assignment3_sriddle.Models
{
    public class MovieCastVM
    {
        public Movie Movie { get; set; }
        public List<Cast> Casts { get; set;}
        public TweetsVM TweetsVM { get; set; }
    }
}
