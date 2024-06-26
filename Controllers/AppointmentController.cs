using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clinic.Models;
using Microsoft.AspNetCore.Authorization;

namespace Clinic.Controllers
{
    [CustomAuthorizationFilter("admin", "user")]
    public class AppointmentController : Controller
    {
        private readonly ClinicDbContext _context;

        public AppointmentController(ClinicDbContext context)
        {
            _context = context;
        }

        // GET: Appointment
        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["IdSortParam"] = string.IsNullOrEmpty(sortOrder) || sortOrder == "id_asc" ? "id_desc" : "id_asc";
            ViewData["DescSortParam"] = sortOrder == "description_asc" ? "description_desc" : "description_asc";
            ViewData["DoctorSortParam"] = sortOrder == "doctor_asc" ? "doctor_desc" : "doctor_asc";
            ViewData["PatientSortParam"] = sortOrder == "patient_asc" ? "patient_desc" : "patient_asc";

            IQueryable<Appointment> appointments = _context.Appointments.Include(a => a.Doctor).Include(a => a.Patient);

            if (!string.IsNullOrEmpty(searchString))
            {
                appointments = appointments.Where(a => a.Id.ToString().Contains(searchString) || a.Description.Contains(searchString) || (a.Doctor.LastName + " " + a.Doctor.FirstName + " " + a.Doctor.MiddleName).Contains(searchString) ||(a.Patient.LastName + " " + a.Patient.FirstName + " " + a.Patient.MiddleName).Contains(searchString));
            }

            switch (sortOrder)
            {
                case "description_asc":
                    appointments = appointments.OrderBy(a => a.Description);
                    break;
                case "description_desc":
                    appointments = appointments.OrderByDescending(a => a.Description);
                    break;
                case "doctor_asc":
                    appointments = appointments.OrderBy(a => a.Doctor.LastName).ThenBy(a => a.Doctor.FirstName).ThenBy(a => a.Doctor.MiddleName);
                    break;
                case "doctor_desc":
                    appointments = appointments.OrderByDescending(a => a.Doctor.LastName).ThenByDescending(a => a.Doctor.FirstName).ThenByDescending(a => a.Doctor.MiddleName);
                    break;
                case "patient_asc":
                    appointments = appointments.OrderBy(a => a.Patient.LastName).ThenBy(a => a.Patient.FirstName).ThenBy(a => a.Patient.MiddleName);
                    break;
                case "patient_desc":
                    appointments = appointments.OrderByDescending(a => a.Patient.LastName).ThenByDescending(a => a.Patient.FirstName).ThenByDescending(a => a.Patient.MiddleName);
                    break;
                case "id_desc":
                    appointments = appointments.OrderByDescending(a => a.Id);
                    break;
                default:
                    appointments = appointments.OrderBy(a => a.Id);
                    break;
            }

            List<Appointment> appointmentList = await appointments.ToListAsync();
            return View(appointmentList);
        }

        // GET: Appointment/Create
        [CustomAuthorizationFilter("admin")]
        public IActionResult Create()
        {
            ViewBag.Doctors = _context.Doctors.Select(d => new { Id = d.Id, FullName = $"{d.LastName} {d.FirstName} {d.MiddleName}" }).ToList();
            ViewBag.Patients = _context.Patients.Select(p => new { Id = p.Id, FullName = $"{p.LastName} {p.FirstName} {p.MiddleName}" }).ToList();
            return View();
        }

        // POST: Appointment/Create
        [CustomAuthorizationFilter("admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,DoctorId,PatientId")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appointment);
        }

        // GET: Appointment/Edit/5
        [CustomAuthorizationFilter("admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            // Получаем список врачей и пациентов и сохраняем их в ViewBag
            ViewBag.Doctors = await _context.Doctors.Select(d => new { Id = d.Id, FullName = $"{d.LastName} {d.FirstName} {d.MiddleName}" }).ToListAsync();
            ViewBag.Patients = await _context.Patients.Select(p => new { Id = p.Id, FullName = $"{p.LastName} {p.FirstName} {p.MiddleName}" }).ToListAsync();

            return View(appointment);
        }

        // POST: Appointment/Edit/5
        [CustomAuthorizationFilter("admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,DoctorId,PatientId")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
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
            return View(appointment);
        }

        // GET: Appointment/Delete/5
        [CustomAuthorizationFilter("admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.Include(a => a.Doctor).Include(a => a.Patient).FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointment/Delete/5
        [CustomAuthorizationFilter("admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }

        [CustomAuthorizationFilter("admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }
    }
}
