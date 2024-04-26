using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clinic.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clinic.Controllers
{
    public class RegistryController : Controller
    {
        private readonly ClinicDbContext _context;

        public RegistryController(ClinicDbContext context)
        {
            _context = context;
        }

        // GET: Registry
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["SubdivisionNameSortParam"] = sortOrder == "subdivision_asc" ? "subdivision_desc" : "subdivision_asc";
            ViewData["DepartmentNumberSortParam"] = sortOrder == "department_asc" ? "department_desc" : "department_asc";
            ViewData["HeadSortParam"] = sortOrder == "head_asc" ? "head_desc" : "head_asc";
            ViewData["OrganizationTypeSortParam"] = sortOrder == "organization_asc" ? "organization_desc" : "organization_asc";
            ViewData["StreetSortParam"] = sortOrder == "street_asc" ? "street_desc" : "street_asc";
            ViewData["HouseNumberSortParam"] = sortOrder == "house_asc" ? "house_desc" : "house_asc";
            ViewData["CitySortParam"] = sortOrder == "city_asc" ? "city_desc" : "city_asc";
            ViewData["RecordingMethodSortParam"] = sortOrder == "recording_asc" ? "recording_desc" : "recording_asc";

            var registries = _context.Registries
                .Include(r => r.RecordingMethod)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchString))
            {
                registries = registries.Where(r =>
                    r.SubdivisionName.Contains(searchString) ||
                    r.DepartmentNumber.ToString().Contains(searchString) ||
                    r.Head.Contains(searchString) ||
                    r.OrganizationType.Contains(searchString) ||
                    r.Street.Contains(searchString) ||
                    r.HouseNumber.Contains(searchString) ||
                    r.City.Contains(searchString) ||
                    r.RecordingMethod.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "subdivision_desc":
                    registries = registries.OrderByDescending(r => r.SubdivisionName);
                    break;
                case "department_desc":
                    registries = registries.OrderByDescending(r => r.DepartmentNumber);
                    break;
                case "head_desc":
                    registries = registries.OrderByDescending(r => r.Head);
                    break;
                case "organization_desc":
                    registries = registries.OrderByDescending(r => r.OrganizationType);
                    break;
                case "street_desc":
                    registries = registries.OrderByDescending(r => r.Street);
                    break;
                case "house_desc":
                    registries = registries.OrderByDescending(r => r.HouseNumber);
                    break;
                case "city_desc":
                    registries = registries.OrderByDescending(r => r.City);
                    break;
                case "recording_desc":
                    registries = registries.OrderByDescending(r => r.RecordingMethod.Name);
                    break;
                case "subdivision_asc":
                    registries = registries.OrderBy(r => r.SubdivisionName);
                    break;
                case "department_asc":
                    registries = registries.OrderBy(r => r.DepartmentNumber);
                    break;
                case "head_asc":
                    registries = registries.OrderBy(r => r.Head);
                    break;
                case "organization_asc":
                    registries = registries.OrderBy(r => r.OrganizationType);
                    break;
                case "street_asc":
                    registries = registries.OrderBy(r => r.Street);
                    break;
                case "house_asc":
                    registries = registries.OrderBy(r => r.HouseNumber);
                    break;
                case "city_asc":
                    registries = registries.OrderBy(r => r.City);
                    break;
                case "recording_asc":
                    registries = registries.OrderBy(r => r.RecordingMethod.Name);
                    break;
                default:
                    registries = registries.OrderBy(r => r.SubdivisionName);
                    break;
            }

            return View(await registries.ToListAsync());
        }

        // GET: Registry/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registry = await _context.Registries
            .Include(r => r.RecordingMethod)
            .FirstOrDefaultAsync(m => m.Id == id);
            if (registry == null)
            {
                return NotFound();
            }

            return View(registry);
        }

        // GET: Registry/Create
        public IActionResult Create()
        {
            ViewData["RecordingMethodId"] = new SelectList(_context.RecordingMethods, "Id", "Name");
            return View();
        }

        // POST: Registry/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubdivisionName,DepartmentNumber,Head,OrganizationType,Street,HouseNumber,City,RecordingMethodId")] Registry registry)
        {
            if (ModelState.IsValid)
            {
                int maxId = await _context.Registries.MaxAsync(d => (int?)d.Id) ?? 0;

                // Увеличиваем id на 1 и присваиваем его новой записи
                registry.Id = maxId + 1;
                _context.Add(registry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RecordingMethodId"] = new SelectList(_context.RecordingMethods, "Id", "Name", registry.RecordingMethodId);
            return View(registry);
        }

        // GET: Registry/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registry = await _context.Registries.FindAsync(id);
            if (registry == null)
            {
                return NotFound();
            }
            ViewData["RecordingMethodId"] = new SelectList(_context.RecordingMethods, "Id", "Name", registry.RecordingMethodId);
            return View(registry);
        }

        // POST: Registry/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SubdivisionName,DepartmentNumber,Head,OrganizationType,Street,HouseNumber,City,RecordingMethodId")] Registry registry)
        {
            if (id != registry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistryExists(registry.Id))
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
            ViewData["RecordingMethodId"] = new SelectList(_context.RecordingMethods, "Id", "Name", registry.RecordingMethodId);
            return View(registry);
        }

        // GET: Registry/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registry = await _context.Registries
            .Include(r => r.RecordingMethod)
            .FirstOrDefaultAsync(m => m.Id == id);
            if (registry == null)
            {
                return NotFound();
            }

            return View(registry);
        }

        // POST: Registry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registry = await _context.Registries.FindAsync(id);
            _context.Registries.Remove(registry);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistryExists(int id)
        {
            return _context.Registries.Any(e => e.Id == id);
        }
    }
}
