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
                var appointments = _context.Appointments
                    .Where(a => a.PatientId == patient.Id)
                    .Include(a => a.Doctor)
                    .ToList();

                var appointmentIds = appointments.Select(a => a.Id).ToList();
                var tickets = _context.Tickets.Where(t => appointmentIds.Contains(t.Id)).ToList();

                return View(Tuple.Create(appointments, tickets)); // Передать записи в представление
            }
            else
            {
                ViewBag.ErrorMessage = "Упс... Пациента с таким номером телефона не существует";
                return View(Tuple.Create(new List<Appointment>(), new List<Ticket>()));
            }
        }
    }
}
