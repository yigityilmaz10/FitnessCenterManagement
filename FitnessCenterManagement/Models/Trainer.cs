using System.ComponentModel.DataAnnotations;

namespace FitnessCenterManagement.Models
{
    public class Trainer
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        public int ServiceId { get; set; }
        public Service? Service { get; set; }


        public string AvailableHours { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }
    }
}
