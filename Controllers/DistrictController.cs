using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clinic.Models;

namespace Clinic.Controllers
{
    public class DistrictController : Controller
    {
        private readonly ClinicDbContext _context;

        public DistrictController(ClinicDbContext context)
        {
            _context = context;
        }

        // GET: District
        public IActionResult Index(string sortOrder, string searchString)
        {
            IQueryable<District> districts = _context.Districts.Include(d => d.Doctor);

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                districts = districts.Where(d =>
                    d.Id.ToString().Contains(searchString) ||
                    d.DistrictNumber.ToString().Contains(searchString) ||
                    (d.Doctor.LastName.ToLower() + " " + d.Doctor.FirstName.ToLower() + " " + d.Doctor.MiddleName.ToLower()).Contains(searchString)
                );
            }

            ViewData["CurrentSort"] = sortOrder;
            ViewData["IdSortParam"] = string.IsNullOrEmpty(sortOrder) || sortOrder == "id_asc" ? "id_desc" : "id_asc";
            ViewData["DistrictNumberSortParam"] = sortOrder == "districtnumber_asc" ? "districtnumber_desc" : "districtnumber_asc";
            ViewData["DoctorSortParam"] = sortOrder == "doctor_asc" ? "doctor_desc" : "doctor_asc";

            switch (sortOrder)
            {
                case "districtnumber_asc":
                    districts = districts.OrderBy(d => d.DistrictNumber);
                    break;
                case "districtnumber_desc":
                    districts = districts.OrderByDescending(d => d.DistrictNumber);
                    break;
                case "doctor_asc":
                    districts = districts.OrderBy(d => d.Doctor.MiddleName).ThenBy(d => d.Doctor.FirstName).ThenBy(d => d.Doctor.LastName);
                    break;
                case "doctor_desc":
                    districts = districts.OrderByDescending(d => d.Doctor.MiddleName).ThenByDescending(d => d.Doctor.FirstName).ThenByDescending(d => d.Doctor.LastName);
                    break;
                case "id_desc":
                    districts = districts.OrderByDescending(d => d.Id);
                    break;
                default:
                    districts = districts.OrderBy(d => d.Id);
                    break;
            }

            return View(districts.ToList());
        }

        // GET: District/Create
        public IActionResult Create()
        {
            ViewBag.Doctors = _context.Doctors.ToList();
            
            return View();
        }

        // POST: District/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(District district)
        {
            if (ModelState.IsValid)
            {
                var maxId = _context.Districts.Max(d => (int?)d.Id) ?? 0;
                district.Id = maxId + 1;
                _context.Add(district);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(district);
        }

        // GET: District/Edit/5
        public IActionResult Edit(int? id)
        {
            ViewBag.Doctors = _context.Doctors.ToList();
            if (id == null)
            {
                return NotFound();
            }

            var district = _context.Districts.Find(id);
            if (district == null)
            {
                return NotFound();
            }

            ViewBag.Doctors = _context.Doctors.ToList();
            return View(district);
        }

        // POST: District/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, District district)
        {
            if (id != district.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(district);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DistrictExists(district.Id))
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
            return View(district);
        }

        // GET: District/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var district = _context.Districts.Include(ds => ds.Doctor).FirstOrDefault(m => m.Id == id);
            if (district == null)
            {
                return NotFound();
            }

            return View(district);
        }

        // POST: District/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var district = _context.Districts.Find(id);
            _context.Districts.Remove(district);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool DistrictExists(int id)
        {
            return _context.Districts.Any(d => d.Id == id);
        }
    }
}
