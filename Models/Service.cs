using System.ComponentModel.DataAnnotations;

namespace FitnessCenterManagement.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int DurationMinutes { get; set; }

        public decimal Price { get; set; }
    }
}
