// Dosya: FitnessCenterManagement/Controllers/AppointmentsController.cs

using FitnessCenterManagement.Data;
using FitnessCenterManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims; // ClaimTypes için
using System.Threading.Tasks;

namespace FitnessCenterManagement.Controllers
{
    // YETKİLENDİRME: Sadece 'Member' rolündeki kullanıcılar erişebilir.
    // Admin, kendi üyeliği varsa buraya erişebilir, ancak asıl randevu admin panelden yönetilebilir.
    [Authorize(Roles = "Member,Admin")]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AppointmentsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Appointments (Üyenin Kendi Randevuları)
        public async Task<IActionResult> Index()
        {
            // O an giriş yapmış üyenin Identity ID'sini al
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Sadece bu üyeye ait randevuları, ilişkili Antrenör ve Hizmet bilgileriyle birlikte getir.
            var appointments = _context.Appointments
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .Where(a => a.MemberId == currentUserId);

            return View(await appointments.ToListAsync());
        }

        // GET: Appointments/Create (Randevu Alma Formu)
        public IActionResult Create()
        {
            // Dropdownlar için gerekli verileri hazırlar
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName");
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name");
            return View();
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrainerId,ServiceId,AppointmentDate")] Appointment appointment)
        {
            // 1. Üye Bilgisini Ekleme
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            appointment.MemberId = currentUserId;

            // 2. Randevu Onayını Ayarlama (Ödev gereksinimi: Randevu Onay Mekanizması)
            appointment.IsApproved = false; // Yeni randevu varsayılan olarak onaysız başlar.

            // 3. Basit Çakışma Kontrolü (Ödev gereksinimi: Önceki randevular dikkate alınmalı)
            // Aynı Antrenörün, aynı saat ve dakikada başka bir randevusu var mı kontrol et.
            var conflict = await _context.Appointments
                .Where(a => a.TrainerId == appointment.TrainerId &&
                            a.AppointmentDate.Date == appointment.AppointmentDate.Date &&
                            a.AppointmentDate.Hour == appointment.AppointmentDate.Hour &&
                            a.AppointmentDate.Minute == appointment.AppointmentDate.Minute)
                .AnyAsync();

            if (conflict)
            {
                ModelState.AddModelError(string.Empty, "Seçtiğiniz saatte antrenörün başka bir randevusu bulunmaktadır. Lütfen farklı bir saat seçiniz.");
            }

            if (ModelState.IsValid && !conflict)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Randevunuz oluşturuldu ve onay bekliyor.";
                return RedirectToAction(nameof(Index));
            }

            // Hata varsa veya çakışma varsa, dropdown'ları tekrar yükle ve formu yeniden göster
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName", appointment.TrainerId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", appointment.ServiceId);
            return View(appointment);
        }

        // Randevuyu Silme (Üye tarafından iptal)
        // Buraya Delete, Details vb. CRUD aksiyonlarını da Trainer/Gym Controller'lardan kopyalayıp ekleyebilirsin.
    }
}