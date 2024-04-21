using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Clinic.Models;
using Microsoft.AspNetCore.Authorization;
using Clinic.Controllers;

[CustomAuthorizationFilter("admin")]
public class DoctorsController : Controller
{
    private readonly ClinicDbContext _context;

    public DoctorsController(ClinicDbContext context)
    {
        _context = context;
    }

    // GET: Doctors
    [HttpGet]
    public async Task<IActionResult> Index(string sortOrder, string searchString)
    {
        ViewData["CurrentSort"] = sortOrder;
        ViewData["IdSortParam"] = string.IsNullOrEmpty(sortOrder) || sortOrder == "id_asc" ? "id_desc" : "id_asc";
        ViewData["FirstNameSortParam"] = sortOrder == "first_name_asc" ? "first_name_desc" : "first_name_asc";
        ViewData["LastNameSortParam"] = sortOrder == "last_name_asc" ? "last_name_desc" : "last_name_asc";
        ViewData["MiddleNameSortParam"] = sortOrder == "middle_name_asc" ? "middle_name_desc" : "middle_name_asc";
        ViewData["RegistrySortParam"] = sortOrder == "registry_asc" ? "registry_desc" : "registry_asc";
        ViewData["SpecialtySortParam"] = sortOrder == "specialty_asc" ? "specialty_desc" : "specialty_asc";

        IQueryable<Doctor> doctors = _context.Doctors.Include(d => d.Registry).Include(d => d.Specialty);

        if (!string.IsNullOrEmpty(searchString))
        {
            doctors = doctors.Where(d => 
                d.Id.ToString().Contains(searchString) ||
                d.FirstName.Contains(searchString) ||
                d.LastName.Contains(searchString) ||
                d.MiddleName.Contains(searchString) ||
                d.RegistryId.ToString().Contains(searchString) ||
                d.SpecialtyId.ToString().Contains(searchString));
        }

        switch (sortOrder)
        {
            case "first_name_asc":
                doctors = doctors.OrderBy(d => d.FirstName);
                break;
            case "first_name_desc":
                doctors = doctors.OrderByDescending(d => d.FirstName);
                break;
            case "last_name_asc":
                doctors = doctors.OrderBy(d => d.LastName);
                break;
            case "last_name_desc":
                doctors = doctors.OrderByDescending(d => d.LastName);
                break;
            case "middle_name_asc":
                doctors = doctors.OrderBy(d => d.MiddleName);
                break;
            case "middle_name_desc":
                doctors = doctors.OrderByDescending(d => d.MiddleName);
                break;
            case "registry_asc":
                doctors = doctors.OrderBy(d => d.RegistryId);
                break;
            case "registry_desc":
                doctors = doctors.OrderByDescending(d => d.RegistryId);
                break;
            case "specialty_asc":
                doctors = doctors.OrderBy(d => d.SpecialtyId);
                break;
            case "specialty_desc":
                doctors = doctors.OrderByDescending(d => d.SpecialtyId);
                break;
            case "id_desc":
                doctors = doctors.OrderByDescending(d => d.Id);
                break;
            default:
                doctors = doctors.OrderBy(d => d.Id);
                break;
        }

        List<Doctor> doctorList = await doctors.ToListAsync();
        return View(doctorList);
    }

    // GET: Doctors/Create
    public async Task<IActionResult> CreateAsync()
    {
        ViewBag.Registries = await _context.Registries.ToListAsync();
        ViewBag.Specialties = await _context.DoctorSpecialties.ToListAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FirstName,LastName,MiddleName,RegistryId,SpecialtyId")] Doctor doctor)
    {
        if (ModelState.IsValid)
        {
            // Находим максимальное значение Id в таблице Doctors
            int maxId = await _context.Doctors.MaxAsync(d => (int?)d.Id) ?? 0;

            // Увеличиваем id на 1 и присваиваем его новой записи
            doctor.Id = maxId + 1;

            _context.Add(doctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(doctor);
    }

    // GET: Doctors/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var doctor = await _context.Doctors.FindAsync(id);
        if (doctor == null)
        {
            return NotFound();
        }
        ViewBag.Registries = await _context.Registries.ToListAsync();
        ViewBag.Specialties = await _context.DoctorSpecialties.ToListAsync();
        return View(doctor);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,MiddleName,RegistryId,SpecialtyId")] Doctor doctor)
    {
        if (id != doctor.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(doctor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(doctor.Id))
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
        return View(doctor);
    }

    // GET: Doctors/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(m => m.Id == id);
        if (doctor == null)
        {
            return NotFound();
        }
        ViewBag.Registries = await _context.Registries.ToListAsync();
        ViewBag.Specialties = await _context.DoctorSpecialties.ToListAsync();
        return View(doctor);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        _context.Doctors.Remove(doctor);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DoctorExists(int id)
    {
        return _context.Doctors.Any(e => e.Id == id);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var doctor = await _context.Doctors
            .Include(d => d.Registry)
            .Include(d => d.Specialty)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (doctor == null)
        {
            return NotFound();
        }

        return View(doctor);
    }

}
