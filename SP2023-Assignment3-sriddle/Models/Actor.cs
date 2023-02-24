using Microsoft.Build.Framework;

namespace SP2023_Assignment3_sriddle.Models
{
    public class Actor
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Gender { get; set; }
        public int? Age { get; set; }
        public string? IMDBLink { get; set; }
        public byte[]? Photo { get; set; }
    }
}
