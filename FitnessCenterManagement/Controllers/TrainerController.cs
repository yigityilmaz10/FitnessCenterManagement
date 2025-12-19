using FitnessCenterManagement.Data;
using FitnessCenterManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TrainerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // LIST
        // =========================
        public async Task<IActionResult> Index()
        {
            var trainers = await _context.Trainers.ToListAsync();
            return View(trainers);
        }

        // =========================
        // CREATE (GET)
        // =========================
        public IActionResult Create()
        {
            return View();
        }

        // =========================
        // CREATE (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Trainer trainer)
        {
            if (!ModelState.IsValid)
            {
                return View(trainer);
            }

            _context.Trainers.Add(trainer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // EDIT (GET)
        // =========================
        public async Task<IActionResult> Edit(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
                return NotFound();

            return View(trainer);
        }

        // =========================
        // EDIT (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Trainer trainer)
        {
            if (id != trainer.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(trainer);

            try
            {
                _context.Update(trainer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                bool exists = await _context.Trainers.AnyAsync(t => t.Id == trainer.Id);
                if (!exists)
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // DELETE (GET)
        // =========================
        public async Task<IActionResult> Delete(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
                return NotFound();

            return View(trainer);
        }

        // =========================
        // DELETE (POST)
        // =========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
                return NotFound();

            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
