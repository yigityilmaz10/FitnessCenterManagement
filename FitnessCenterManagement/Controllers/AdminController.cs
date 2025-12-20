using FitnessCenterManagement.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            ViewBag.TrainerCount = await _context.Trainers.CountAsync();
            ViewBag.ServiceCount = await _context.Services.CountAsync();
            ViewBag.AppointmentCount = await _context.Appointments.CountAsync();
            ViewBag.PendingAppointments = await _context.Appointments
                .CountAsync(a => !a.IsApproved);

            return View();
        }
    }
}
