using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clinic.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clinic.Controllers
{
    public class PatientsController : Controller
    {
        private readonly ClinicDbContext _context;

        public PatientsController(ClinicDbContext context)
        {
            _context = context;
        }

        // GET: Patients
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["LastNameSortParam"] = sortOrder == "last_name_asc" ? "last_name_desc" : "last_name_asc";
            ViewData["FirstNameSortParam"] = sortOrder == "first_name_asc" ? "first_name_desc" : "first_name_asc";
            ViewData["MiddleNameSortParam"] = sortOrder == "middle_name_asc" ? "middle_name_desc" : "middle_name_asc";
            ViewData["BirthDateSortParam"] = sortOrder == "birth_date_asc" ? "birth_date_desc" : "birth_date_asc";
            ViewData["PhoneSortParam"] = sortOrder == "phone_asc" ? "phone_desc" : "phone_asc";
            ViewData["AddressSortParam"] = sortOrder == "address_asc" ? "address_desc" : "address_asc";
            ViewData["DistrictSortParam"] = sortOrder == "district_asc" ? "district_desc" : "district_asc";

            var patients = _context.Patients
                .Include(p => p.District)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchString))
            {
                patients = patients.Where(p =>
                    p.FirstName.Contains(searchString) ||
                    p.LastName.Contains(searchString) ||
                    p.MiddleName.Contains(searchString) ||
                    p.Address.Contains(searchString) ||
                    p.Phone.Contains(searchString) ||
                    p.District.DistrictNumber.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "last_name_desc":
                    patients = patients.OrderByDescending(p => p.LastName);
                    break;
                case "first_name_desc":
                    patients = patients.OrderByDescending(p => p.FirstName);
                    break;
                case "middle_name_desc":
                    patients = patients.OrderByDescending(p => p.MiddleName);
                    break;
                case "birth_date_desc":
                    patients = patients.OrderByDescending(p => p.BirthDate);
                    break;
                case "phone_desc":
                    patients = patients.OrderByDescending(p => p.Phone);
                    break;
                case "address_desc":
                    patients = patients.OrderByDescending(p => p.Address);
                    break;
                case "district_desc":
                    patients = patients.OrderByDescending(p => p.District.DistrictNumber);
                    break;
                case "last_name_asc":
                    patients = patients.OrderBy(p => p.LastName);
                    break;
                case "first_name_asc":
                    patients = patients.OrderBy(p => p.FirstName);
                    break;
                case "middle_name_asc":
                    patients = patients.OrderBy(p => p.MiddleName);
                    break;
                case "birth_date_asc":
                    patients = patients.OrderBy(p => p.BirthDate);
                    break;
                case "phone_asc":
                    patients = patients.OrderBy(p => p.Phone);
                    break;
                case "address_asc":
                    patients = patients.OrderBy(p => p.Address);
                    break;
                case "district_asc":
                    patients = patients.OrderBy(p => p.District.DistrictNumber);
                    break;
                default:
                    patients = patients.OrderBy(p => p.LastName);
                    break;
            }

            return View(await patients.ToListAsync());
        }


        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
            .Include(p => p.District)
            .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "DistrictNumber");
            return View();
        }

        // POST: Patients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,MiddleName,BirthDate,Address,Phone,DistrictId")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                int maxId = await _context.Patients.MaxAsync(d => (int?)d.Id) ?? 0;

                // Увеличиваем id на 1 и присваиваем его новой записи
                patient.Id = maxId + 1;
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "DistrictNumber", patient.DistrictId);
            return View(patient);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "DistrictNumber", patient.DistrictId);
            return View(patient);
        }

        // POST: Patients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,MiddleName,BirthDate,Address,Phone,DistrictId")] Patient patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
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
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "DistrictNumber", patient.DistrictId);
            return View(patient);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
            .Include(p => p.District)
            .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}