using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnessCenterManagement.Models
{
    public class Gym
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Address { get; set; }

        [Required]
        public string WorkingHours { get; set; }

        public List<Service> Services { get; set; }
        public List<Trainer> Trainers { get; set; }
    }
}
