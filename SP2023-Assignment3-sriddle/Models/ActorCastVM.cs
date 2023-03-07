namespace SP2023_Assignment3_sriddle.Models
{
    public class ActorCastVM
    {
        public Actor Actor { get; set; }
        public List<Cast> Casts { get; set; }
        public TweetsVM TweetsVM { get; set; }
    }
}
