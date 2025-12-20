using FitnessCenterManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterManagement.Controllers
{
    [Authorize]
    public class AIController : Controller
    {
        // =========================
        // FORM
        // =========================
        public IActionResult Index()
        {
            return View();
        }

        // =========================
        // SONUÇ
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Result(AIExerciseRequest model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            // 🔒 BACKEND HEDEF KONTROLÜ
            var allowedGoals = new[]
            {
                "Kilo Vermek",
                "Kas Kazanmak",
                "Kondisyon Artırmak",
                "Esneklik Geliştirmek",
                "Genel Sağlık"
            };

            if (!allowedGoals.Contains(model.Goal))
            {
                ModelState.AddModelError("Goal", "Geçersiz hedef seçimi.");
                return View("Index", model);
            }

            // 🤖 AI SİMÜLASYONU (KARAR DESTEK)
            string recommendation = model.Goal switch
            {
                "Kilo Vermek" =>
                    "Haftada 4 gün orta tempolu kardiyo (koşu, bisiklet) ve " +
                    "2 gün hafif ağırlık antrenmanı önerilir. Kalori açığı oluşturulmalıdır.",

                "Kas Kazanmak" =>
                    "Haftada 5 gün bölgesel ağırlık antrenmanı ve " +
                    "yüksek proteinli beslenme önerilir. Dinlenme günleri ihmal edilmemelidir.",

                "Kondisyon Artırmak" =>
                    "HIIT, interval koşu ve yüzme gibi " +
                    "dayanıklılık artırıcı egzersizler haftada 3–4 gün önerilir.",

                "Esneklik Geliştirmek" =>
                    "Yoga, pilates ve statik esneme egzersizleri " +
                    "haftada en az 3 gün uygulanmalıdır.",

                "Genel Sağlık" =>
                    "Dengeli ağırlık antrenmanı, hafif kardiyo ve " +
                    "düzenli yürüyüş içeren bir program önerilir.",

                _ =>
                    "Genel bir egzersiz programı önerilir."
            };

            ViewBag.Recommendation = recommendation;
            return View(model);
        }
    }
}
