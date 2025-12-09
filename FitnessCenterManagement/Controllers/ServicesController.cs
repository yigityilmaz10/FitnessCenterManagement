// Dosya: FitnessCenterManagement/Controllers/ServicesController.cs

using FitnessCenterManagement.Data;
using FitnessCenterManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessCenterManagement.Controllers
{
    // YETKİLENDİRME: Sadece 'Admin' rolündeki kullanıcılar erişebilir.
    [Authorize(Roles = "Admin")]
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Services
        // Tüm hizmetleri listeler (READ)
        public async Task<IActionResult> Index()
        {
            // Hizmetleri çekerken, ilişkili oldukları Gym (Spor Salonu) verisini de çekiyoruz.
            var applicationDbContext = _context.Services.Include(s => s.Gym);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Gym) // İlişkili salon bilgisini de çek
                .FirstOrDefaultAsync(m => m.Id == id);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // GET: Services/Create
        // Yeni hizmet oluşturma formunu gösterir (CREATE)
        public IActionResult Create()
        {
            // Hangi salona ait olduğunu seçmek için dropdown (açılır liste) verisini hazırlar.
            ViewData["GymId"] = new SelectList(_context.Gyms, "Id", "Name");
            return View();
        }

        // POST: Services/Create
        // Formdan gelen veriyi kaydeder (CREATE)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Duration,Price,GymId")] Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Eğer doğrulama başarısız olursa, formu yeniden gösterirken Gym listesini tekrar yükle
            ViewData["GymId"] = new SelectList(_context.Gyms, "Id", "Name", service.GymId);
            return View(service);
        }

        // GET: Services/Edit/5
        // Düzenleme formunu gösterir (UPDATE)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            // Dropdown için Gym listesini tekrar yükle, mevcut GymId'yi seçili olarak işaretle
            ViewData["GymId"] = new SelectList(_context.Gyms, "Id", "Name", service.GymId);
            return View(service);
        }

        // POST: Services/Edit/5
        // Düzenleme verisini kaydeder (UPDATE)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Duration,Price,GymId")] Service service)
        {
            if (id != service.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Services.Any(e => e.Id == service.Id))
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
            ViewData["GymId"] = new SelectList(_context.Gyms, "Id", "Name", service.GymId);
            return View(service);
        }

        // GET: Services/Delete/5
        // Silme onay sayfasını gösterir
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Gym) // İlişkili salon bilgisini de çek
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Services/Delete/5
        // Silme işlemini gerçekleştirir (DELETE)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}