using Clinic.Models;
using Clinic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<IActionResult> IndexAsync(string sortOrder, string searchString1, string searchString2, bool? filterDoctor1, int? selectedDoctor1, bool? filterSpecialty, int? selectedSpecialty, DateOnly? selectedDateFrom, DateOnly? selectedDateTo, bool? filterDateFrom, bool? filterDateTo, bool filterDoctor, int? selectedDoctor, bool filterDay, string selectedDay)
        {
            var userEmail = User.Identity.Name; // Получить email текущего пользователя
            var patient = _context.Patients.FirstOrDefault(p => p.Phone == userEmail); // Найти пациента по номеру телефона

            if (patient != null)
            {
                var tickets = _context.Tickets
                    .Where(t => t.PatientId == patient.Id)
                    .Include(t => t.Doctor)
                    .ThenInclude(d => d.Specialty)
                    .Include(t => t.Patient)
                    .AsQueryable();

                // Фильтрация
                if (filterDoctor1 == true && selectedDoctor1.HasValue)
                {
                    tickets = tickets.Where(a => a.DoctorId == selectedDoctor1.Value);
                }

                if (filterSpecialty == true && selectedSpecialty.HasValue)
                {
                    tickets = tickets.Where(d => d.Doctor.SpecialtyId == selectedSpecialty.Value);
                }

                if (filterDateFrom == true && selectedDateFrom.HasValue)
                {
                    tickets = tickets.Where(ds => ds.AppointmentDate >= selectedDateFrom);
                }

                if (filterDateTo == true && selectedDateTo.HasValue)
                {
                    tickets = tickets.Where(ds => ds.AppointmentDate <= selectedDateTo);
                }

                // Поиск
                if (!string.IsNullOrEmpty(searchString2))
                {
                    tickets = tickets.Where(t =>
                        t.Doctor.FirstName.Contains(searchString2) ||
                        t.Doctor.MiddleName.Contains(searchString2) ||
                        t.Doctor.LastName.Contains(searchString2) ||
                        t.Doctor.Specialty.Name.Contains(searchString2) ||
                        t.AppointmentDate.HasValue && t.AppointmentDate.Value.ToString().Contains(searchString2) ||
                        t.AppointmentTime.HasValue && t.AppointmentTime.Value.ToString().Contains(searchString2));
                }

                // Сортировка
                switch (sortOrder)
                {
                    case "doctor_desc":
                        tickets = tickets.OrderByDescending(t => t.Doctor.LastName);
                        break;
                    case "appointment_date_desc":
                        tickets = tickets.OrderByDescending(t => t.AppointmentDate);
                        break;
                    case "appointment_time_desc":
                        tickets = tickets.OrderByDescending(t => t.AppointmentTime);
                        break;
                    case "doctor_asc":
                        tickets = tickets.OrderBy(t => t.Doctor.LastName);
                        break;
                    case "appointment_date_asc":
                        tickets = tickets.OrderBy(t => t.AppointmentDate);
                        break;
                    case "appointment_time_asc":
                        tickets = tickets.OrderBy(t => t.AppointmentTime);
                        break;
                    default:
                        tickets = tickets.OrderBy(t => t.Doctor.LastName);
                        break;
                }

                ViewData["CurrentSort"] = sortOrder;
                ViewData["DoctorSortParam"] = sortOrder == "doctor_asc" ? "doctor_desc" : "doctor_asc";
                ViewData["AppointmentDateSortParam"] = sortOrder == "appointment_date_asc" ? "appointment_date_desc" : "appointment_date_asc";
                ViewData["AppointmentTimeSortParam"] = sortOrder == "appointment_time_asc" ? "appointment_time_desc" : "appointment_time_asc";

                ViewBag.Doctors = _context.Doctors.Select(d => new { Id = d.Id, FullName = $"{d.LastName} {d.FirstName} {d.MiddleName}" }).ToList();
                ViewBag.Specialties = _context.DoctorSpecialties.ToList();
                ViewData["UserFullName"] = GetUserFullName();

                ViewData["CurrentSort"] = sortOrder;
                ViewData["DoctorSortParam"] = sortOrder == "doctor_asc" ? "doctor_desc" : "doctor_asc";
                ViewData["DayOfWeekSortParam"] = sortOrder == "dayofweek_asc" ? "dayofweek_desc" : "dayofweek_asc";
                ViewData["StartTimeSortParam"] = sortOrder == "starttime_asc" ? "starttime_desc" : "starttime_asc";
                ViewData["EndTimeSortParam"] = sortOrder == "endtime_asc" ? "endtime_desc" : "endtime_asc";

                ViewBag.Days = _context.Schedules.Select(p => p.DayOfWeek).Distinct().ToList();

                var schedules = _context.Schedules
                    .Include(s => s.Doctor)
                    .AsNoTracking();

                if (!string.IsNullOrEmpty(searchString1))
                {
                    searchString1 = searchString1.ToLower();
                    schedules = schedules.Where(s =>
                        s.Doctor.LastName.ToLower().Contains(searchString1) ||
                        s.Doctor.FirstName.ToLower().Contains(searchString1) ||
                        s.Doctor.MiddleName.ToLower().Contains(searchString1) ||
                        s.DayOfWeek.ToLower().Contains(searchString1) ||
                        s.StartTime.ToString().ToLower().Contains(searchString1) ||
                        s.EndTime.ToString().ToLower().Contains(searchString1));
                }

                if (filterDay)
                {
                    schedules = schedules.Where(p => p.DayOfWeek == selectedDay);
                }

                if (filterDoctor)
                {
                    schedules = schedules.Where(p => p.DoctorId == selectedDoctor.Value);
                }

                var viewModel = new HomeViewModel
                {
                    Tickets = tickets.ToList(),
                    Schedules = await schedules.ToListAsync()
                };

                return View(viewModel);
            }
            else
            {
                ViewBag.ErrorMessage = "Упс... Пациента с таким номером телефона не существует";
                return View(new HomeViewModel { Tickets = new List<Ticket>() });
            }
        }

        public IActionResult Create()
        {
            var userEmail = User.Identity.Name;
            var patient = _context.Patients.FirstOrDefault(p => p.Phone == userEmail);

            if (patient != null)
            {
                ViewData["DoctorId"] = new SelectList(_context.Doctors.Include(d => d.Specialty), "Id", "FullName");
                ViewBag.Patient = patient;
                return View();
            }
            else
            {
                ViewBag.ErrorMessage = "Упс... Пациента с таким номером телефона не существует";
                return View(new Ticket());
            }
        }


        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DoctorId,AppointmentDate,AppointmentTime,PatientId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                int maxId = await _context.Tickets.MaxAsync(d => (int?)d.Id) ?? 0;

                // Увеличиваем id на 1 и присваиваем его новой записи
                ticket.Id = maxId + 1;

                // Проверяем, не занято ли выбранное время у врача
                DateTime dateTime = ticket.AppointmentDate.Value.ToDateTime(TimeOnly.MinValue);
                string dayOfWeek = dateTime.DayOfWeek.ToString();

                var doctorSchedule = await _context.Schedules
                    .Where(s => s.DoctorId == ticket.DoctorId &&
                                s.DayOfWeek == dayOfWeek &&
                                ticket.AppointmentTime >= s.StartTime &&
                                ticket.AppointmentTime <= s.EndTime)
                    .FirstOrDefaultAsync();

                if (doctorSchedule == null)
                {
                    ModelState.AddModelError("AppointmentTime", "Выбранное время не соответствует расписанию врача");
                    ViewData["DoctorId"] = new SelectList(_context.Doctors.Include(d => d.Specialty), "Id", "FullName", ticket.DoctorId);
                    return View(ticket);
                }

                // Проверяем, не занято ли выбранное время в других талонах
                var isTimeBusy = await _context.Tickets
                    .AnyAsync(t => t.DoctorId == ticket.DoctorId &&
                                   t.AppointmentDate == ticket.AppointmentDate &&
                                   t.AppointmentTime == ticket.AppointmentTime);

                if (isTimeBusy)
                {
                    ModelState.AddModelError("AppointmentTime", "Выбранное время уже занято");
                    ViewData["DoctorId"] = new SelectList(_context.Doctors.Include(d => d.Specialty), "Id", "FullName", ticket.DoctorId);
                    return View(ticket);
                }

                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexAsync));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors.Include(d => d.Specialty), "Id", "FullName", ticket.DoctorId);
            return View(ticket);
        }
        private string GetUserFullName()
        {
            var userEmail = User.Identity.Name;
            var patient = _context.Patients.FirstOrDefault(p => p.Phone == userEmail);

            if (patient != null)
            {
                return $"{patient.FirstName} {patient.MiddleName}";
            }

            return string.Empty;
        }
    }
}
