using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clinic.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clinic.Controllers
{
    public class ReasonsForVisitController : Controller
    {
        private readonly ClinicDbContext _context;

        public ReasonsForVisitController(ClinicDbContext context)
        {
            _context = context;
        }

        // GET: ReasonsForVisit
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["PatientSortParam"] = sortOrder == "patient_asc" ? "patient_desc" : "patient_asc";
            ViewData["DescriptionSortParam"] = sortOrder == "description_asc" ? "description_desc" : "description_asc";

            var reasonsForVisit = _context.ReasonsForVisits
                .Include(r => r.Patient)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchString))
            {
                reasonsForVisit = reasonsForVisit.Where(r =>
                    r.Patient.FirstName.Contains(searchString) ||
                    r.Patient.LastName.Contains(searchString) ||
                    r.Patient.MiddleName.Contains(searchString) ||
                    r.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "patient_desc":
                    reasonsForVisit = reasonsForVisit.OrderByDescending(r => r.Patient.LastName);
                    break;
                case "description_desc":
                    reasonsForVisit = reasonsForVisit.OrderByDescending(r => r.Description);
                    break;
                case "patient_asc":
                    reasonsForVisit = reasonsForVisit.OrderBy(r => r.Patient.LastName);
                    break;
                case "description_asc":
                    reasonsForVisit = reasonsForVisit.OrderBy(r => r.Description);
                    break;
                default:
                    reasonsForVisit = reasonsForVisit.OrderBy(r => r.Patient.LastName);
                    break;
            }

            return View(await reasonsForVisit.ToListAsync());
        }

        // GET: ReasonsForVisit/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reasonsForVisit = await _context.ReasonsForVisits
                .Include(r => r.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reasonsForVisit == null)
            {
                return NotFound();
            }

            return View(reasonsForVisit);
        }

        // GET: ReasonsForVisit/Create
        public IActionResult Create()
        {
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FullName");
            return View();
        }

        // POST: ReasonsForVisit/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,PatientId")] ReasonsForVisit reasonsForVisit)
        {
            if (ModelState.IsValid)
            {
                int maxId = await _context.ReasonsForVisits.MaxAsync(d => (int?)d.Id) ?? 0;

                // Увеличиваем id на 1 и присваиваем его новой записи
                reasonsForVisit.Id = maxId + 1;
                _context.Add(reasonsForVisit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FullName", reasonsForVisit.PatientId);
            return View(reasonsForVisit);
        }

        // GET: ReasonsForVisit/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reasonsForVisit = await _context.ReasonsForVisits.FindAsync(id);
            if (reasonsForVisit == null)
            {
                return NotFound();
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FullName", reasonsForVisit.PatientId);
            return View(reasonsForVisit);
        }

        // POST: ReasonsForVisit/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,PatientId")] ReasonsForVisit reasonsForVisit)
        {
            if (id != reasonsForVisit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reasonsForVisit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReasonsForVisitExists(reasonsForVisit.Id))
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
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FullName", reasonsForVisit.PatientId);
            return View(reasonsForVisit);
        }

        // GET: ReasonsForVisit/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reasonsForVisit = await _context.ReasonsForVisits
                .Include(r => r.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reasonsForVisit == null)
            {
                return NotFound();
            }

            return View(reasonsForVisit);
        }

        // POST: ReasonsForVisit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reasonsForVisit = await _context.ReasonsForVisits.FindAsync(id);
            _context.ReasonsForVisits.Remove(reasonsForVisit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReasonsForVisitExists(int id)
        {
            return _context.ReasonsForVisits.Any(e => e.Id == id);
        }
    }
}
