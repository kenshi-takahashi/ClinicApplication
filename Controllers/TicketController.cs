using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clinic.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clinic.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ClinicDbContext _context;

        public TicketsController(ClinicDbContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index(string sortOrder, string searchString, bool? filterDoctor, int? selectedDoctor, bool? filterSpecialty, int? selectedSpecialty, DateOnly? selectedDateFrom, DateOnly? selectedDateTo, bool? filterDateFrom, bool? filterDateTo, bool? filterPatient, int? selectedPatient)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DoctorSortParam"] = sortOrder == "doctor_asc" ? "doctor_desc" : "doctor_asc";
            ViewData["AppointmentDateSortParam"] = sortOrder == "appointment_date_asc" ? "appointment_date_desc" : "appointment_date_asc";
            ViewData["AppointmentTimeSortParam"] = sortOrder == "appointment_time_asc" ? "appointment_time_desc" : "appointment_time_asc";
            ViewData["PatientSortParam"] = sortOrder == "patient_asc" ? "patient_desc" : "patient_asc";

            ViewBag.Doctors = _context.Doctors.Select(d => new { Id = d.Id, FullName = $"{d.LastName} {d.FirstName} {d.MiddleName}" }).ToList();
            ViewBag.Patients = _context.Patients.Select(p => new { Id = p.Id, FullName = $"{p.LastName} {p.FirstName} {p.MiddleName}" }).ToList();
            ViewBag.Specialties = await _context.DoctorSpecialties.ToListAsync();

            var tickets = _context.Tickets
                .Include(t => t.Doctor)
                .ThenInclude(d => d.Specialty)
                .Include(d => d.Patient)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchString))
            {
                tickets = tickets.Where(t =>
                    t.Doctor.LastName.Contains(searchString) ||
                    t.Doctor.FirstName.Contains(searchString) ||
                    t.Doctor.MiddleName.Contains(searchString) ||
                    t.Doctor.Specialty.Name.Contains(searchString) ||
                    t.AppointmentDate.HasValue && t.AppointmentDate.Value.ToString().Contains(searchString) ||
                    t.AppointmentTime.HasValue && t.AppointmentTime.Value.ToString().Contains(searchString));
            }

            if (filterDoctor == true && selectedDoctor.HasValue)
            {
                tickets = tickets.Where(a => a.DoctorId == selectedDoctor.Value);
            }

            if (filterSpecialty == true)
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

            if (filterPatient == true && selectedPatient.HasValue)
            {
                tickets = tickets.Where(a => a.PatientId == selectedPatient.Value);
            }

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
                case "patient_asc":
                    tickets = tickets.OrderBy(a => a.Patient.LastName).ThenBy(a => a.Patient.FirstName).ThenBy(a => a.Patient.MiddleName);
                    break;
                case "patient_desc":
                    tickets = tickets.OrderByDescending(a => a.Patient.LastName).ThenByDescending(a => a.Patient.FirstName).ThenByDescending(a => a.Patient.MiddleName);
                    break;
                default:
                    tickets = tickets.OrderBy(t => t.Doctor.LastName);
                    break;
            }

            int ticketsCount = await tickets.CountAsync();
            ViewBag.ticketsCount = ticketsCount;
            return View(await tickets.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Doctor)
                .ThenInclude(d => d.Specialty)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["DoctorId"] = new SelectList(_context.Doctors.Include(d => d.Specialty), "Id", "FullName");
            ViewBag.Patients = _context.Patients.Select(p => new { Id = p.Id, FullName = $"{p.LastName} {p.FirstName} {p.MiddleName}" }).ToList();
            return View();
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors.Include(d => d.Specialty), "Id", "FullName", ticket.DoctorId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors.Include(d => d.Specialty), "Id", "FullName", ticket.DoctorId);
            ViewBag.Patients = await _context.Patients.Select(p => new { Id = p.Id, FullName = $"{p.LastName} {p.FirstName} {p.MiddleName}" }).ToListAsync();
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DoctorId,AppointmentDate,AppointmentTime,PatientId")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            ViewData["DoctorId"] = new SelectList(_context.Doctors.Include(d => d.Specialty), "Id", "FullName", ticket.DoctorId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Doctor)
                .ThenInclude(d => d.Specialty)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
