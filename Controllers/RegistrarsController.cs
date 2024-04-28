using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clinic.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clinic.Controllers
{
    public class RegistrarsController : Controller
    {
        private readonly ClinicDbContext _context;

        public RegistrarsController(ClinicDbContext context)
        {
            _context = context;
        }

        // GET: Registrars
        public async Task<IActionResult> Index(string sortOrder, string searchString, bool filterLastName, bool filterFirstName, bool filterMiddleName, bool filterSubdivisionName, string selectedLastName, string selectedFirstName, string selectedMiddleName, int? selectedSubdivisionName)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["LastNameSortParam"] = sortOrder == "lastname_asc" ? "lastname_desc" : "lastname_asc";
            ViewData["FirstNameSortParam"] = sortOrder == "firstname_asc" ? "firstname_desc" : "firstname_asc";
            ViewData["MiddleNameSortParam"] = sortOrder == "middlename_asc" ? "middlename_desc" : "middlename_asc";
            ViewData["SubdivisionSortParam"] = sortOrder == "subdivision_asc" ? "subdivision_desc" : "subdivision_asc";

            ViewBag.LastNames = _context.Registrars.Select(p => p.LastName).Distinct().ToList();
            ViewBag.MiddleNames = _context.Registrars.Select(p => p.MiddleName).Distinct().ToList();
            ViewBag.FirstNames = _context.Registrars.Select(p => p.FirstName).Distinct().ToList();
            ViewBag.SubdivisionNames = _context.Registries.Select(p => new { Id = p.Id, Name = $"{p.SubdivisionName}"}).ToList();

            var registrars = _context.Registrars
                .Include(r => r.Registry)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchString))
            {
                registrars = registrars.Where(r =>
                    r.LastName.Contains(searchString) ||
                    r.FirstName.Contains(searchString) ||
                    r.MiddleName.Contains(searchString) ||
                    r.Registry.SubdivisionName.Contains(searchString));
            }

            
            if (filterLastName)
            {
                registrars = registrars.Where(p => p.LastName == selectedLastName);
            }

            if (filterFirstName)
            {
                registrars = registrars.Where(p => p.FirstName == selectedFirstName);
            }

            if (filterMiddleName)
            {
                registrars = registrars.Where(p => p.MiddleName == selectedMiddleName);
            }

            if (filterSubdivisionName)
            {
                registrars = registrars.Where(p => p.RegistryId == selectedSubdivisionName.Value);
            }

            switch (sortOrder)
            {
                case "lastname_desc":
                    registrars = registrars.OrderByDescending(r => r.LastName);
                    break;
                case "firstname_desc":
                    registrars = registrars.OrderByDescending(r => r.FirstName);
                    break;
                case "middlename_desc":
                    registrars = registrars.OrderByDescending(r => r.MiddleName);
                    break;
                case "subdivision_desc":
                    registrars = registrars.OrderByDescending(r => r.Registry.SubdivisionName);
                    break;
                case "lastname_asc":
                    registrars = registrars.OrderBy(r => r.LastName);
                    break;
                case "firstname_asc":
                    registrars = registrars.OrderBy(r => r.FirstName);
                    break;
                case "middlename_asc":
                    registrars = registrars.OrderBy(r => r.MiddleName);
                    break;
                case "subdivision_asc":
                    registrars = registrars.OrderBy(r => r.Registry.SubdivisionName);
                    break;
                default:
                    registrars = registrars.OrderBy(r => r.LastName);
                    break;
            }

            int registrarsCount = await registrars.CountAsync();
            ViewBag.registrarsCount = registrarsCount;

            return View(await registrars.ToListAsync());
        }

        // GET: Registrars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registrar = await _context.Registrars
                .Include(r => r.Registry)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registrar == null)
            {
                return NotFound();
            }

            return View(registrar);
        }

        // GET: Registrars/Create
        public IActionResult Create()
        {
            ViewData["RegistryId"] = new SelectList(_context.Registries, "Id", "SubdivisionName");
            return View();
        }

        // POST: Registrars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LastName,FirstName,MiddleName,RegistryId")] Registrar registrar)
        {
            if (ModelState.IsValid)
            {
                int maxId = await _context.Registrars.MaxAsync(d => (int?)d.Id) ?? 0;

                // Увеличиваем id на 1 и присваиваем его новой записи
                registrar.Id = maxId + 1;
                _context.Add(registrar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RegistryId"] = new SelectList(_context.Registries, "Id", "SubdivisionName", registrar.RegistryId);
            return View(registrar);
        }

        // GET: Registrars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registrar = await _context.Registrars.FindAsync(id);
            if (registrar == null)
            {
                return NotFound();
            }
            ViewData["RegistryId"] = new SelectList(_context.Registries, "Id", "SubdivisionName", registrar.RegistryId);
            return View(registrar);
        }

        // POST: Registrars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,FirstName,MiddleName,RegistryId")] Registrar registrar)
        {
            if (id != registrar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registrar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrarExists(registrar.Id))
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
            ViewData["RegistryId"] = new SelectList(_context.Registries, "Id", "SubdivisionName", registrar.RegistryId);
            return View(registrar);
        }

        // GET: Registrars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registrar = await _context.Registrars
            .Include(r => r.Registry)
            .FirstOrDefaultAsync(m => m.Id == id);
            if (registrar == null)
            {
                return NotFound();
            }

            return View(registrar);
        }

        // POST: Registrars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registrar = await _context.Registrars.FindAsync(id);
            _context.Registrars.Remove(registrar);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistrarExists(int id)
        {
            return _context.Registrars.Any(e => e.Id == id);
        }
    }
}
