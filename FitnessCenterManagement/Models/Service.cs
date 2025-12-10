//services.cs
using System.ComponentModel.DataAnnotations;

namespace FitnessCenterManagement.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int Duration { get; set; } // dakika

        public decimal Price { get; set; }

        public int GymId { get; set; }
        public Gym Gym { get; set; }
    }
}
