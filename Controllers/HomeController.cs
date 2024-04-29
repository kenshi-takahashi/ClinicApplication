using Clinic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Clinic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ClinicDbContext _context;

        public HomeController(ClinicDbContext context)
        {
            _context = context;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Index()
        {
            var userEmail = User.Identity.Name; // Получить email текущего пользователя
            var patient = _context.Patients.FirstOrDefault(p => p.Phone == userEmail); // Найти пациента по номеру телефона

            if (patient != null)
            {
                var tickets = _context.Tickets
                    .Where(t => t.PatientId == patient.Id)
                    .Include(t => t.Doctor)
                    .Include(t => t.Patient)
                    .ToList();

                return View(tickets); // Передать записи в представление
            }
            else
            {
                ViewBag.ErrorMessage = "Упс... Пациента с таким номером телефона не существует";
                return View(new List<Ticket>());
            }
        }
    }
}
