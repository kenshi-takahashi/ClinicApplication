using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clinic.Models;
using System.Security.Cryptography;

namespace Clinic.Controllers
{
    public class DoctorSpecialtiesController : Controller
    {
        private readonly ClinicDbContext _context;

        public DoctorSpecialtiesController(ClinicDbContext context)
        {
            _context = context;
        }

        // GET: DoctorSpecialties
        public async Task<IActionResult> Index(string sortOrder, string searchString, string selectedName, string selectedDescription, bool? filterName, bool? filterDescription)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) || sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewData["DescriptionSortParam"] = sortOrder == "description_asc" ? "description_desc" : "description_asc";
            // Add more sort parameters for other fields if needed

            IQueryable<DoctorSpecialty> specialties = _context.DoctorSpecialties;

            if (!string.IsNullOrEmpty(searchString))
            {
                specialties = specialties.Where(s => s.Name.Contains(searchString) || s.Description.Contains(searchString));
                // Add more search criteria if needed
            }

            if (filterName == true)
            {
                specialties = specialties.Where(ds => ds.Name == selectedName);
            }

            if (filterDescription == true)
            {
                specialties = specialties.Where(ds => ds.Description == selectedDescription);
            }

            switch (sortOrder)
            {
                case "name_asc":
                    specialties = specialties.OrderBy(s => s.Name);
                    break;
                case "name_desc":
                    specialties = specialties.OrderByDescending(s => s.Name);
                    break;
                case "description_asc":
                    specialties = specialties.OrderBy(s => s.Description);
                    break;
                case "description_desc":
                    specialties = specialties.OrderByDescending(s => s.Description);
                    break;
                // Add more sorting options for other fields if needed
                default:
                    specialties = specialties.OrderBy(s => s.Id);
                    break;
            }

            int specialtiesCount = specialties.Count();
            ViewBag.specialtiesCount = specialtiesCount;
            ViewBag.Name = _context.DoctorSpecialties.Select(d => d.Name).Distinct().ToList();
            ViewBag.Description = _context.DoctorSpecialties.Select(d => d.Description).Distinct().ToList();
            return View(await specialties.ToListAsync());
        }

        // GET: DoctorSpecialties/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DoctorSpecialties/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] DoctorSpecialty doctorSpecialty)
        {
            if (ModelState.IsValid)
            {
                // Находим максимальное значение Id в таблице DoctorSpecialties
                int maxId = await _context.DoctorSpecialties.MaxAsync(d => (int?)d.Id) ?? 0;

                // Увеличиваем id на 1 и присваиваем его новой записи
                doctorSpecialty.Id = maxId + 1;

                _context.Add(doctorSpecialty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(doctorSpecialty);
        }


        // GET: DoctorSpecialties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorSpecialty = await _context.DoctorSpecialties.FindAsync(id);
            if (doctorSpecialty == null)
            {
                return NotFound();
            }
            return View(doctorSpecialty);
        }

        // POST: DoctorSpecialties/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] DoctorSpecialty doctorSpecialty)
        {
            if (id != doctorSpecialty.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctorSpecialty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorSpecialtyExists(doctorSpecialty.Id))
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
            return View(doctorSpecialty);
        }

        // GET: DoctorSpecialties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorSpecialty = await _context.DoctorSpecialties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctorSpecialty == null)
            {
                return NotFound();
            }

            return View(doctorSpecialty);
        }

        // POST: DoctorSpecialties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctorSpecialty = await _context.DoctorSpecialties.FindAsync(id);
            _context.DoctorSpecialties.Remove(doctorSpecialty);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorSpecialtyExists(int id)
        {
            return _context.DoctorSpecialties.Any(e => e.Id == id);
        }
    }
}
