using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnessCenterManagement.Models
{
    public class Trainer
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        public string Expertise { get; set; } // kas geliştirme, yoga vb.

        public string AvailableHours { get; set; }

        public int GymId { get; set; }
        public Gym Gym { get; set; }

        public List<Appointment> Appointments { get; set; }
    }
}
