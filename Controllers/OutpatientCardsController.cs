using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clinic.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clinic.Controllers
{
    public class OutpatientCardsController : Controller
    {
        private readonly ClinicDbContext _context;

        public OutpatientCardsController(ClinicDbContext context)
        {
            _context = context;
        }

        // GET: OutpatientCards
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CardNumberSortParam"] = sortOrder == "card_number_desc" ? "card_number_asc" : "card_number_desc";
            ViewData["PatientSortParam"] = sortOrder == "patient_desc" ? "patient_asc" : "patient_desc";

            var outpatientCards = _context.OutpatientCards
                .Include(o => o.Patient)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchString))
            {
                outpatientCards = outpatientCards.Where(o =>
                    o.CardNumber.Contains(searchString) ||
                    (o.Patient.MiddleName + " " + o.Patient.FirstName + " " + o.Patient.LastName).Contains(searchString));
            }

            switch (sortOrder)
            {
                case "card_number_desc":
                    outpatientCards = outpatientCards.OrderByDescending(o => o.CardNumber);
                    break;
                case "patient_desc":
                    outpatientCards = outpatientCards.OrderByDescending(o => o.Patient.MiddleName)
                        .ThenByDescending(o => o.Patient.FirstName)
                        .ThenByDescending(o => o.Patient.LastName);
                    break;
                case "card_number_asc":
                    outpatientCards = outpatientCards.OrderBy(o => o.CardNumber);
                    break;
                case "patient_asc":
                    outpatientCards = outpatientCards.OrderBy(o => o.Patient.MiddleName)
                        .ThenBy(o => o.Patient.FirstName)
                        .ThenBy(o => o.Patient.LastName);
                    break;
                default:
                    outpatientCards = outpatientCards.OrderBy(o => o.CardNumber);
                    break;
            }

            return View(await outpatientCards.ToListAsync());
        }

        // GET: OutpatientCards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outpatientCard = await _context.OutpatientCards
                .Include(o => o.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (outpatientCard == null)
            {
                return NotFound();
            }

            return View(outpatientCard);
        }

        // GET: OutpatientCards/Create
        public IActionResult Create()
        {
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FullName");
            return View();
        }

        // POST: OutpatientCards/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CardNumber,PatientId")] OutpatientCard outpatientCard)
        {
            if (ModelState.IsValid)
            {
                int maxId = await _context.OutpatientCards.MaxAsync(d => (int?)d.Id) ?? 0;

                // Увеличиваем id на 1 и присваиваем его новой записи
                outpatientCard.Id = maxId + 1;
                _context.Add(outpatientCard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FullName", outpatientCard.PatientId);
            return View(outpatientCard);
        }

        // GET: OutpatientCards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outpatientCard = await _context.OutpatientCards.FindAsync(id);
            if (outpatientCard == null)
            {
                return NotFound();
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FullName", outpatientCard.PatientId);
            return View(outpatientCard);
        }

        // POST: OutpatientCards/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CardNumber,PatientId")] OutpatientCard outpatientCard)
        {
            if (id != outpatientCard.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(outpatientCard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OutpatientCardExists(outpatientCard.Id))
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
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FullName", outpatientCard.PatientId);
            return View(outpatientCard);
        }

        // GET: OutpatientCards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outpatientCard = await _context.OutpatientCards
                .Include(o => o.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (outpatientCard == null)
            {
                return NotFound();
            }

            return View(outpatientCard);
        }

        // POST: OutpatientCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var outpatientCard = await _context.OutpatientCards.FindAsync(id);
            _context.OutpatientCards.Remove(outpatientCard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OutpatientCardExists(int id)
        {
            return _context.OutpatientCards.Any(e => e.Id == id);
        }
    }
}
