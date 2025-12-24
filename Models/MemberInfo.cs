using System.ComponentModel.DataAnnotations;

namespace FitnessCenterManagement.Models
{
    public class MemberInfo
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public int Height { get; set; }   // cm
        public int Weight { get; set; }   // kg
        public string Goal { get; set; }  // kilo verme, kas, vb
    }
}
