using FitnessCenterManagement.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterManagement.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FitnessApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FitnessApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // 1️⃣ TÜM ANTRENÖRLER
        // GET: api/FitnessApi/trainers
        // =========================
        [HttpGet("trainers")]
        public async Task<IActionResult> GetAllTrainers()
        {
            var trainers = await _context.Trainers
                .Select(t => new
                {
                    t.Id,
                    t.FullName,
                    t.Specialty,
                    t.AvailableHours
                })
                .ToListAsync();

            return Ok(trainers);
        }

        // =========================
        // 2️⃣ BELİRLİ TARİHTE UYGUN ANTRENÖRLER
        // GET: api/FitnessApi/available-trainers?date=2025-01-10T14:00
        // =========================
        [HttpGet("available-trainers")]
        public async Task<IActionResult> GetAvailableTrainers(DateTime date)
        {
            var busyTrainerIds = await _context.Appointments
                .Where(a => a.AppointmentDate == date)
                .Select(a => a.TrainerId)
                .ToListAsync();

            var availableTrainers = await _context.Trainers
                .Where(t => !busyTrainerIds.Contains(t.Id))
                .Select(t => new
                {
                    t.Id,
                    t.FullName,
                    t.Specialty
                })
                .ToListAsync();

            return Ok(availableTrainers);
        }

        // =========================
        // 3️⃣ ÜYENİN RANDEVULARI
        // GET: api/FitnessApi/user-appointments/{userId}
        // =========================
        [HttpGet("user-appointments/{userId}")]
        public async Task<IActionResult> GetUserAppointments(string userId)
        {
            var appointments = await _context.Appointments
                .Where(a => a.UserId == userId)
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .Select(a => new
                {
                    a.Id,
                    Trainer = a.Trainer.FullName,
                    Service = a.Service.Name,
                    a.AppointmentDate,
                    a.IsApproved
                })
                .ToListAsync();

            return Ok(appointments);
        }
    }
}
