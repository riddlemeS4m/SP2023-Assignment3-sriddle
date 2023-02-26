using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace SP2023_Assignment3_sriddle.Models
{
    public class CastVM
    {
        public Cast Cast { get; set; }
        public SelectList ActorNames { get; set; }  
        public SelectList MovieNames { get; set; }  
    }
}
