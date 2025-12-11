namespace FitnessCenterManagement.Models
{
    public class Appointment
    {
<<<<<<< HEAD
        public int Id { get; set; }

        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }

        public int ServiceId { get; set; }
        public Service Service { get; set; }

        [Required]
        public string MemberId { get; set; }  // Identity User ID.

        public DateTime AppointmentDate { get; set; }

        public bool IsApproved { get; set; }
=======
>>>>>>> a94b8cc19ae3069b7555901b1d76b947f79d028e
    }
}
