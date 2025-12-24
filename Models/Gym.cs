using System.ComponentModel.DataAnnotations;

namespace FitnessCenterManagement.Models
{
    public class Gym
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string WorkingHours { get; set; }
    }
}
