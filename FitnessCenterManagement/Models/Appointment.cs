using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessCenterManagement.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        // 🔴 ZORUNLU ALANLAR (ID'LER)
        [Required]
        public int TrainerId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        public bool IsApproved { get; set; }

        // 🟢 NAVIGATION PROPERTY'LER (POST'TA NULL OLABİLİR)
        public Trainer? Trainer { get; set; }
        public Service? Service { get; set; }
    }
}
