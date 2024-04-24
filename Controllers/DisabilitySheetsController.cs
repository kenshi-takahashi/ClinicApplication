using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Clinic.Models;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Controllers
{
    public class DisabilitySheetController : Controller
    {
        private readonly ClinicDbContext _context;

        public DisabilitySheetController(ClinicDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string sortOrder, string searchString)
        {
            IQueryable<DisabilitySheet> disabilitySheets  = _context.DisabilitySheets.Include(ds => ds.Doctor);
            if (!string.IsNullOrEmpty(searchString))
            {
                disabilitySheets = disabilitySheets.Where(a =>
                a.Id.ToString().Contains(searchString) ||
                a.SheetNumber.Contains(searchString) || 
                (a.Doctor.LastName + " " + a.Doctor.FirstName + " " + a.Doctor.MiddleName).Contains(searchString) || 
                a.IssueDate.HasValue && a.IssueDate.Value.ToString("dd.MM.yyyy").Contains(searchString)
                );
            }
            ViewData["CurrentSort"] = sortOrder;
            ViewData["IdSortParam"] = string.IsNullOrEmpty(sortOrder) || sortOrder == "id_asc" ? "id_desc" : "id_asc";
            ViewData["SheetNumberSortParam"] = sortOrder == "sheetnumber_asc" ? "sheetnumber_desc" : "sheetnumber_asc";
            ViewData["DoctorSortParam"] = sortOrder == "doctor_asc" ? "doctor_desc" : "doctor_asc";
            ViewData["DateSortParam"] = sortOrder == "date_asc" ? "date_desc" : "date_asc";

            switch (sortOrder)
            {
                case "sheetnumber_asc":
                    disabilitySheets = disabilitySheets.OrderBy(ds => ds.SheetNumber);
                    break;
                case "sheetnumber_desc":
                    disabilitySheets = disabilitySheets.OrderByDescending(ds => ds.SheetNumber);
                    break;
                case "doctor_asc":
                    disabilitySheets = disabilitySheets.OrderBy(ds => ds.Doctor.LastName).ThenBy(ds => ds.Doctor.FirstName).ThenBy(ds => ds.Doctor.MiddleName);
                    break;
                case "doctor_desc":
                    disabilitySheets = disabilitySheets.OrderByDescending(ds => ds.Doctor.LastName).ThenByDescending(ds => ds.Doctor.FirstName).ThenByDescending(ds => ds.Doctor.MiddleName);
                    break;
                case "date_asc":
                    disabilitySheets = disabilitySheets.OrderBy(ds => ds.IssueDate);
                    break;
                case "date_desc":
                    disabilitySheets = disabilitySheets.OrderByDescending(ds => ds.IssueDate);
                    break;
                case "id_desc":
                    disabilitySheets = disabilitySheets.OrderByDescending(ds => ds.Id);
                    break;
                default:
                    disabilitySheets = disabilitySheets.OrderBy(ds => ds.Id);
                    break;
            }

            return View(disabilitySheets);
        }

        public IActionResult Create()
        {
            ViewBag.Doctors = _context.Doctors.Select(d => new { Id = d.Id, FullName = $"{d.LastName} {d.FirstName} {d.MiddleName}" }).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DisabilitySheet disabilitySheet)
        {
            if (ModelState.IsValid)
            {
                // Определяем максимальный Id и увеличиваем на 1
                int maxId = _context.DisabilitySheets.Max(ds => (int?)ds.Id) ?? 0;
                disabilitySheet.Id = maxId + 1;

                _context.DisabilitySheets.Add(disabilitySheet);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Doctors = _context.Doctors.Select(d => new { Id = d.Id, FullName = $"{d.LastName} {d.FirstName} {d.MiddleName}" }).ToList();
            return View(disabilitySheet);
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Doctors = _context.Doctors.Select(d => new { Id = d.Id, FullName = $"{d.LastName} {d.FirstName} {d.MiddleName}" }).ToList();
            var disabilitySheet = _context.DisabilitySheets.Find(id);
            if (disabilitySheet == null)
            {
                return NotFound();
            }
            return View(disabilitySheet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, DisabilitySheet disabilitySheet)
        {
            if (id != disabilitySheet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(disabilitySheet);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Doctors = _context.Doctors.Select(d => new { Id = d.Id, FullName = $"{d.LastName} {d.FirstName} {d.MiddleName}" }).ToList();
            return View(disabilitySheet);
        }

        public IActionResult Delete(int id)
        {
            var disabilitySheet = _context.DisabilitySheets.Include(ds => ds.Doctor).FirstOrDefault(ds => ds.Id == id);
            if (disabilitySheet == null)
            {
                return NotFound();
            }
            return View(disabilitySheet);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var disabilitySheet = _context.DisabilitySheets.Find(id);
            _context.DisabilitySheets.Remove(disabilitySheet);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
