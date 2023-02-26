using System.ComponentModel.DataAnnotations.Schema;

namespace SP2023_Assignment3_sriddle.Models
{
    public class Cast
    {
        public int Id { get; set; }
        [ForeignKey("Movie")]
        public int? MovieId { get; set; }

        [ForeignKey("Actor")]
        public int? ActorId { get; set; }

        public string? CharacterName { get; set; }
        public Movie? Movie { get; set; }
        public Actor? Actor { get; set; }

    }
}
