using FitnessCenterManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterManagement.Controllers
{
    [Authorize]
    public class AIController : Controller
    {
        // FORM
        public IActionResult Index()
        {
            return View();
        }

        // SONUÇ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Result(AIExerciseRequest model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            string recommendation;

            if (model.Goal.ToLower().Contains("kilo"))
            {
                recommendation =
                    "Haftada 4 gün kardiyo (koşu, bisiklet), " +
                    "3 gün hafif ağırlık + kalori açığı önerilir.";
            }
            else if (model.Goal.ToLower().Contains("kas"))
            {
                recommendation =
                    "Haftada 5 gün ağırlık antrenmanı, " +
                    "yüksek proteinli beslenme önerilir.";
            }
            else
            {
                recommendation =
                    "Dengeli ağırlık + kardiyo programı uygundur.";
            }

            ViewBag.Recommendation = recommendation;
            return View(model);
        }
    }
}
