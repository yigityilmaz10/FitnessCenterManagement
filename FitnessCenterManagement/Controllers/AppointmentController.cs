using FitnessCenterManagement.Data;
using FitnessCenterManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterManagement.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AppointmentController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // =========================
        // ADMIN – TÜM RANDEVULAR
        // =========================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .ToListAsync();

            return View(appointments);
        }

        // =========================
        // ÜYE – KENDİ RANDEVULARI
        // =========================
        public async Task<IActionResult> MyAppointments()
        {
            var userId = _userManager.GetUserId(User);

            var appointments = await _context.Appointments
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .Where(a => a.UserId == userId)
                .ToListAsync();

            return View(appointments);
        }

        // =========================
        // CREATE (GET)
        // =========================
        public IActionResult Create()
        {
            ViewBag.Trainers = new SelectList(_context.Trainers, "Id", "FullName");
            ViewBag.Services = new SelectList(_context.Services, "Id", "Name");
            return View();
        }

        // =========================
        // CREATE (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            appointment.UserId = _userManager.GetUserId(User);
            appointment.IsApproved = false;

            // ⛔ ÇAKIŞMA KONTROLÜ (PDF'DE ARANAN LINQ)
            bool conflict = await _context.Appointments.AnyAsync(a =>
                a.TrainerId == appointment.TrainerId &&
                a.AppointmentDate == appointment.AppointmentDate
            );

            if (conflict)
                ModelState.AddModelError("", "Bu antrenör bu tarih ve saatte dolu.");

            if (!ModelState.IsValid)
            {
                ViewBag.Trainers = new SelectList(_context.Trainers, "Id", "FullName");
                ViewBag.Services = new SelectList(_context.Services, "Id", "Name");
                return View(appointment);
            }

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MyAppointments));
        }

        // =========================
        // EDIT (GET)
        // =========================
        public async Task<IActionResult> Edit(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            ViewBag.Trainers = new SelectList(_context.Trainers, "Id", "FullName", appointment.TrainerId);
            ViewBag.Services = new SelectList(_context.Services, "Id", "Name", appointment.ServiceId);

            return View(appointment);
        }

        // =========================
        // EDIT (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Appointment appointment)
        {
            if (id != appointment.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Trainers = new SelectList(_context.Trainers, "Id", "FullName", appointment.TrainerId);
                ViewBag.Services = new SelectList(_context.Services, "Id", "Name", appointment.ServiceId);
                return View(appointment);
            }

            _context.Update(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // DELETE (GET)
        // =========================
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // =========================
        // DELETE (POST)
        // =========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
