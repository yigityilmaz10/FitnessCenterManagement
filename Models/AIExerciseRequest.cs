using System.ComponentModel.DataAnnotations;

namespace FitnessCenterManagement.Models
{
    public class AIExerciseRequest
    {
        [Required]
        public int Height { get; set; }

        [Required]
        public int Weight { get; set; }

        [Required]
        public string Goal { get; set; }
    }
}
