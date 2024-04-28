using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clinic.Models;

namespace Clinic.Controllers
{
    public class RecordingMethodsController : Controller
    {
        private readonly ClinicDbContext _context;

        public RecordingMethodsController(ClinicDbContext context)
        {
            _context = context;
        }

        // GET: RecordingMethods
        public async Task<IActionResult> Index(string sortOrder, string searchString, bool filterName, bool filterDescription, string selectedName, string selectedDescription)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewData["DescriptionSortParam"] = sortOrder == "description_asc" ? "description_desc" : "description_asc";

            ViewBag.Descriptions = _context.RecordingMethods.Select(a => a.Description).Distinct().ToList();
            ViewBag.Name = _context.RecordingMethods.Select(a => a.Name).Distinct().ToList();

            var recordingMethods = _context.RecordingMethods
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchString))
            {
                recordingMethods = recordingMethods.Where(rm =>
                    rm.Name.Contains(searchString) ||
                    rm.Description.Contains(searchString));
            }

            if (filterName)
            {
                recordingMethods = recordingMethods.Where(p => p.Name == selectedName);
            }

            if (filterDescription)
            {
                recordingMethods = recordingMethods.Where(p => p.Description == selectedDescription);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    recordingMethods = recordingMethods.OrderByDescending(rm => rm.Name);
                    break;
                case "description_desc":
                    recordingMethods = recordingMethods.OrderByDescending(rm => rm.Description);
                    break;
                case "name_asc":
                    recordingMethods = recordingMethods.OrderBy(rm => rm.Name);
                    break;
                case "description_asc":
                    recordingMethods = recordingMethods.OrderBy(rm => rm.Description);
                    break;
                default:
                    recordingMethods = recordingMethods.OrderBy(rm => rm.Name);
                    break;
            }

            int recordingMethodsCount = await recordingMethods.CountAsync();
            ViewBag.recordingMethodsCount = recordingMethodsCount;

            return View(await recordingMethods.ToListAsync());
        }

        // GET: RecordingMethods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recordingMethod = await _context.RecordingMethods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recordingMethod == null)
            {
                return NotFound();
            }

            return View(recordingMethod);
        }

        // GET: RecordingMethods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RecordingMethods/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] RecordingMethod recordingMethod)
        {
            if (ModelState.IsValid)
            {
                int maxId = await _context.RecordingMethods.MaxAsync(d => (int?)d.Id) ?? 0;

                // Увеличиваем id на 1 и присваиваем его новой записи
                recordingMethod.Id = maxId + 1;
                _context.Add(recordingMethod);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recordingMethod);
        }

        // GET: RecordingMethods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recordingMethod = await _context.RecordingMethods.FindAsync(id);
            if (recordingMethod == null)
            {
                return NotFound();
            }
            return View(recordingMethod);
        }

        // POST: RecordingMethods/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] RecordingMethod recordingMethod)
        {
            if (id != recordingMethod.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recordingMethod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecordingMethodExists(recordingMethod.Id))
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
            return View(recordingMethod);
        }

        // GET: RecordingMethods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recordingMethod = await _context.RecordingMethods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recordingMethod == null)
            {
                return NotFound();
            }

            return View(recordingMethod);
        }

        // POST: RecordingMethods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recordingMethod = await _context.RecordingMethods.FindAsync(id);
            _context.RecordingMethods.Remove(recordingMethod);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecordingMethodExists(int id)
        {
            return _context.RecordingMethods.Any(e => e.Id == id);
        }
    }
}
