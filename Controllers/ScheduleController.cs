using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clinic.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clinic.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ClinicDbContext _context;

        public ScheduleController(ClinicDbContext context)
        {
            _context = context;
        }

        // GET: Schedule
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DoctorSortParam"] = sortOrder == "doctor_asc" ? "doctor_desc" : "doctor_asc";
            ViewData["DayOfWeekSortParam"] = sortOrder == "dayofweek_asc" ? "dayofweek_desc" : "dayofweek_asc";
            ViewData["StartTimeSortParam"] = sortOrder == "starttime_asc" ? "starttime_desc" : "starttime_asc";
            ViewData["EndTimeSortParam"] = sortOrder == "endtime_asc" ? "endtime_desc" : "endtime_asc";

            var schedules = _context.Schedules
                .Include(s => s.Doctor)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                schedules = schedules.Where(s =>
                    s.Doctor.LastName.ToLower().Contains(searchString) ||
                    s.Doctor.FirstName.ToLower().Contains(searchString) ||
                    s.Doctor.MiddleName.ToLower().Contains(searchString) ||
                    s.DayOfWeek.ToLower().Contains(searchString) ||
                    s.StartTime.ToString().ToLower().Contains(searchString) ||
                    s.EndTime.ToString().ToLower().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "doctor_desc":
                    schedules = schedules.OrderByDescending(s => s.Doctor.LastName);
                    break;
                case "dayofweek_desc":
                    schedules = schedules.OrderByDescending(s => s.DayOfWeek);
                    break;
                case "starttime_desc":
                    schedules = schedules.OrderByDescending(s => s.StartTime);
                    break;
                case "endtime_desc":
                    schedules = schedules.OrderByDescending(s => s.EndTime);
                    break;
                case "doctor_asc":
                    schedules = schedules.OrderBy(s => s.Doctor.LastName);
                    break;
                case "dayofweek_asc":
                    schedules = schedules.OrderBy(s => s.DayOfWeek);
                    break;
                case "starttime_asc":
                    schedules = schedules.OrderBy(s => s.StartTime);
                    break;
                case "endtime_asc":
                    schedules = schedules.OrderBy(s => s.EndTime);
                    break;
                default:
                    schedules = schedules.OrderBy(s => s.Doctor.LastName);
                    break;
            }

            return View(await schedules.ToListAsync());
        }

        // GET: Schedule/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedule/Create
        public IActionResult Create()
        {
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "FullName");
            return View();
        }

        // POST: Schedule/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DoctorId,DayOfWeek,StartTime,EndTime")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                int maxId = await _context.Schedules.MaxAsync(d => (int?)d.Id) ?? 0;

                // Увеличиваем id на 1 и присваиваем его новой записи
                schedule.Id = maxId + 1;
                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "FullName", schedule.DoctorId);
            return View(schedule);
        }

        // GET: Schedule/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "FullName", schedule.DoctorId);
            return View(schedule);
        }

        // POST: Schedule/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DoctorId,DayOfWeek,StartTime,EndTime")] Schedule schedule)
        {
            if (id != schedule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.Id))
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
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "FullName", schedule.DoctorId);
            return View(schedule);
        }

        // GET: Schedule/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id)
        {
            return _context.Schedules.Any(e => e.Id == id);
        }
    }
}
