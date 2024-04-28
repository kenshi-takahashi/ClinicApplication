using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clinic.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clinic.Controllers
{
    public class MedicalDocumentsController : Controller
    {
        private readonly ClinicDbContext _context;

        public MedicalDocumentsController(ClinicDbContext context)
        {
            _context = context;
        }

        // GET: MedicalDocuments
        public async Task<IActionResult> Index(string sortOrder, string searchString, string selectedName, string selectedDescription, string selectedPatient, bool? filterName, bool? filterDescription, bool? filterPatient)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = sortOrder == "name_desc" ? "name_asc" : "name_desc";
            ViewData["DescriptionSortParam"] = sortOrder == "description_desc" ? "description_asc" : "description_desc";
            ViewData["PatientSortParam"] = sortOrder == "patient_desc" ? "patient_asc" : "patient_desc";

            var medicalDocuments = _context.MedicalDocuments
                .Include(m => m.Patient)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchString))
            {
                medicalDocuments = medicalDocuments.Where(m => m.Name.Contains(searchString) || m.Description.Contains(searchString) || (m.Patient.MiddleName + " " + m.Patient.FirstName + " " + m.Patient.LastName).Contains(searchString));
            }

            if (filterName == true)
            {
                medicalDocuments = medicalDocuments.Where(m => m.Name == selectedName);
            }

            if (filterDescription == true)
            {
                medicalDocuments = medicalDocuments.Where(m => m.Description == selectedDescription);
            }

            if (filterPatient == true)
            {
                medicalDocuments = medicalDocuments.Where(m => (m.Patient.MiddleName + " " + m.Patient.FirstName + " " + m.Patient.LastName) == selectedPatient);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    medicalDocuments = medicalDocuments.OrderByDescending(m => m.Name);
                    break;
                case "description_desc":
                    medicalDocuments = medicalDocuments.OrderByDescending(m => m.Description);
                    break;
                case "description_asc":
                    medicalDocuments = medicalDocuments.OrderBy(m => m.Description);
                    break;
                case "patient_desc":
                    medicalDocuments = medicalDocuments.OrderByDescending(a => a.Patient.MiddleName).ThenByDescending(a => a.Patient.FirstName).ThenByDescending(a => a.Patient.LastName);
                    break;
                case "patient_asc":
                    medicalDocuments = medicalDocuments.OrderBy(a => a.Patient.MiddleName).ThenBy(a => a.Patient.FirstName).ThenBy(a => a.Patient.LastName);
                    break;
                default:
                    medicalDocuments = medicalDocuments.OrderBy(m => m.Name);
                    break;
            }

            int medicalDocumentsCount = medicalDocuments.Count();
            ViewBag.medicalDocumentsCount = medicalDocumentsCount;
            ViewBag.Name = _context.MedicalDocuments.Select(d => d.Name).Distinct().ToList();
            ViewBag.Description = _context.MedicalDocuments.Select(d => d.Description).Distinct().ToList();
            ViewBag.Patient = _context.Patients.Select(d => d.FullName).Distinct().ToList();
            return View(await medicalDocuments.ToListAsync());
        }


        // GET: MedicalDocuments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalDocument = await _context.MedicalDocuments
                .Include(m => m.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicalDocument == null)
            {
                return NotFound();
            }

            return View(medicalDocument);
        }

        // GET: MedicalDocuments/Create
        public IActionResult Create()
        {
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FullName");
            return View();
        }

        // POST: MedicalDocuments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,PatientId")] MedicalDocument medicalDocument)
        {
            if (ModelState.IsValid)
            {
                int maxId = await _context.MedicalDocuments.MaxAsync(d => (int?)d.Id) ?? 0;

                // Увеличиваем id на 1 и присваиваем его новой записи
                medicalDocument.Id = maxId + 1;
                _context.Add(medicalDocument);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FullName", medicalDocument.PatientId);
            return View(medicalDocument);
        }

        // GET: MedicalDocuments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalDocument = await _context.MedicalDocuments.FindAsync(id);
            if (medicalDocument == null)
            {
                return NotFound();
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FullName", medicalDocument.PatientId);
            return View(medicalDocument);
        }

        // POST: MedicalDocuments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,PatientId")] MedicalDocument medicalDocument)
        {
            if (id != medicalDocument.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicalDocument);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicalDocumentExists(medicalDocument.Id))
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
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FullName", medicalDocument.PatientId);
            return View(medicalDocument);
        }

        // GET: MedicalDocuments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalDocument = await _context.MedicalDocuments
                .Include(m => m.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicalDocument == null)
            {
                return NotFound();
            }

            return View(medicalDocument);
        }

        // POST: MedicalDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicalDocument = await _context.MedicalDocuments.FindAsync(id);
            _context.MedicalDocuments.Remove(medicalDocument);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicalDocumentExists(int id)
        {
            return _context.MedicalDocuments.Any(e => e.Id == id);
        }
    }
}
