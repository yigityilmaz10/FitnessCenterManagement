using System.ComponentModel.DataAnnotations;

namespace FitnessCenterManagement.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }

        [Required]
        public int ServiceId { get; set; }
        public Service Service { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        public bool IsApproved { get; set; }
    }
}
