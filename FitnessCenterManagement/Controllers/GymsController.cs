// Dosya: FitnessCenterManagement/Controllers/GymsController.cs

using FitnessCenterManagement.Data;
using FitnessCenterManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessCenterManagement.Controllers
{
    // YETKİLENDİRME: Sadece 'Admin' rolündeki kullanıcılar bu Controller'a erişebilir.
    [Authorize(Roles = "Admin")]
    public class GymsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GymsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Gyms
        // Tüm spor salonlarını listeler (READ)
        public async Task<IActionResult> Index()
        {
            // Asenkron olarak veritabanından tüm Gym nesnelerini çeker
            return View(await _context.Gyms.ToListAsync());
        }

        // GET: Gyms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gym = await _context.Gyms.FirstOrDefaultAsync(m => m.Id == id);
            if (gym == null)
            {
                return NotFound();
            }

            return View(gym);
        }

        // GET: Gyms/Create
        // Yeni salon oluşturma formunu gösterir (CREATE)
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gyms/Create
        // Formdan gelen veriyi kaydeder (CREATE)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,WorkingHours")] Gym gym)
        {
            // ModelState.IsValid: Data Annotation (DataAnnotations) kurallarına göre sunucu tarafı doğrulama (validation) yapar.
            if (ModelState.IsValid)
            {
                _context.Add(gym);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // İşlem başarılıysa listeleme sayfasına yönlendir
            }
            return View(gym);
        }

        // GET: Gyms/Edit/5
        // Düzenleme formunu gösterir (UPDATE)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gym = await _context.Gyms.FindAsync(id);
            if (gym == null)
            {
                return NotFound();
            }
            return View(gym);
        }

        // POST: Gyms/Edit/5
        // Düzenleme verisini kaydeder (UPDATE)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,WorkingHours")] Gym gym)
        {
            if (id != gym.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gym);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Gyms.Any(e => e.Id == gym.Id))
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
            return View(gym);
        }

        // GET: Gyms/Delete/5
        // Silme onay sayfasını gösterir (DELETE)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gym = await _context.Gyms.FirstOrDefaultAsync(m => m.Id == id);
            if (gym == null)
            {
                return NotFound();
            }

            return View(gym);
        }

        // POST: Gyms/Delete/5
        // Silme işlemini gerçekleştirir (DELETE)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gym = await _context.Gyms.FindAsync(id);
            if (gym != null)
            {
                _context.Gyms.Remove(gym);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}