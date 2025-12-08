// Dosya: FitnessCenterManagement/Controllers/TrainersController.cs

using FitnessCenterManagement.Data;
using FitnessCenterManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // SelectList için gerekli
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessCenterManagement.Controllers
{
    // YETKİLENDİRME: Sadece 'Admin' rolündeki kullanıcılar erişebilir.
    [Authorize(Roles = "Admin")]
    public class TrainersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Trainers
        // Tüm antrenörleri listeler (READ)
        public async Task<IActionResult> Index()
        {
            // Antrenörleri çekerken, ilişkili oldukları Gym (Spor Salonu) verisini de çekiyoruz (Eager Loading).
            var applicationDbContext = _context.Trainers.Include(t => t.Gym);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Trainers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainers
                .Include(t => t.Gym) // İlişkili salon bilgisini de çek
                .FirstOrDefaultAsync(m => m.Id == id);

            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // GET: Trainers/Create
        // Yeni antrenör oluşturma formunu gösterir (CREATE)
        public IActionResult Create()
        {
            // Dropdown (açılır liste) için mevcut tüm salonları (Gyms) çek ve ViewData'ya ata.
            ViewData["GymId"] = new SelectList(_context.Gyms, "Id", "Name");
            return View();
        }

        // POST: Trainers/Create
        // Formdan gelen veriyi kaydeder (CREATE)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Expertise,AvailableHours,GymId")] Trainer trainer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Eğer doğrulama başarısız olursa, formu yeniden gösterirken Gym listesini tekrar yükle
            ViewData["GymId"] = new SelectList(_context.Gyms, "Id", "Name", trainer.GymId);
            return View(trainer);
        }

        // GET: Trainers/Edit/5
        // Düzenleme formunu gösterir (UPDATE)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
            {
                return NotFound();
            }

            // Dropdown için Gym listesini tekrar yükle, mevcut GymId'yi seçili olarak işaretle
            ViewData["GymId"] = new SelectList(_context.Gyms, "Id", "Name", trainer.GymId);
            return View(trainer);
        }

        // POST: Trainers/Edit/5
        // Düzenleme verisini kaydeder (UPDATE)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Expertise,AvailableHours,GymId")] Trainer trainer)
        {
            if (id != trainer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Trainers.Any(e => e.Id == trainer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Eğer doğrulama başarısız olursa, formu yeniden gösterirken Gym listesini tekrar yükle
            ViewData["GymId"] = new SelectList(_context.Gyms, "Id", "Name", trainer.GymId);
            return View(trainer);
        }

        // GET: Trainers/Delete/5
        // Silme onay sayfasını gösterir (DELETE)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainers
                .Include(t => t.Gym) // İlişkili salon bilgisini de çek
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // POST: Trainers/Delete/5
        // Silme işlemini gerçekleştirir (DELETE)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer != null)
            {
                _context.Trainers.Remove(trainer);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}