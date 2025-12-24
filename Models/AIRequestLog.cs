using System.ComponentModel.DataAnnotations;

namespace FitnessCenterManagement.Models
{
    public class AIRequestLog
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        [Required]
        public int Height { get; set; }

        [Required]
        public int Weight { get; set; }

        [Required]
        public string Goal { get; set; }

        public string? ResponseText { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
