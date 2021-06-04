using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HumanResources.Data;
using HumanResources.Models;

namespace HumanResources.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly HumanResourcesContext _context;

        public DepartmentsController(HumanResourcesContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index(
                    string sortOrder,
                    string currentFilter,
                    string searchString,
                    int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["EmployeeSortParm"] = sortOrder == "employee" ? "employee_desc" : "employee";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var departments = from d in _context.Departments.Include(e => e.Employee)
                            select d;
            if (!String.IsNullOrEmpty(searchString))
            {
                departments = departments.Where(d => d.Name.Contains(searchString)
                || d.Employee.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    departments = departments.OrderByDescending(d => d.Name);
                    break;
                case "employee":
                    departments = departments.OrderBy(d => d.Employee.Name);
                    break;
                case "employee_desc":
                    departments = departments.OrderByDescending(d => d.Employee.Name);
                    break;
                default:
                    departments = departments.OrderBy(d => d.Name);
                    break;
            }

            int pageSize = 5;
            return View(await PaginatedList<Department>.CreateAsync(departments.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Employee)
                .FirstOrDefaultAsync(m => m.DepartmentID == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            ViewData["HeadID"] = new SelectList(_context.Employees, "ID", "Name");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentID,Name,HeadID,PhoneNumber")] Department department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(department);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Невозможно сохранить изменения. " +
                    "Попробуйте еще раз, и если проблема не исчезнет " +
                    "свяжитесь с системным администратором.");
            }
            ViewData["HeadID"] = new SelectList(_context.Employees, "ID", "Name", department.HeadID);
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            ViewData["HeadID"] = new SelectList(_context.Employees, "ID", "Name", department.HeadID);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DepartmentID,Name,HeadID,PhoneNumber")] Department department)
        {
            if (id != department.DepartmentID)
            {
                return NotFound();
            }

            var departmentToUpdate = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentID == id);
            if (await TryUpdateModelAsync<Department>(
                departmentToUpdate,
                "",
                d => d.Name, d => d.HeadID, d => d.PhoneNumber))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Невозможно сохранить изменения. " +
                    "Попробуйте еще раз, и если проблема не исчезнет " +
                    "свяжитесь с системным администратором.");
                }
            }
            ViewData["HeadID"] = new SelectList(_context.Employees, "ID", "Name", department.HeadID);
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Employee)
                .FirstOrDefaultAsync(m => m.DepartmentID == id);
            if (department == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Удаление не удалось." +
                    "Попробуйте еще раз, и если проблема не исчезнет " +
                    "свяжитесь с системным администратором.";
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Employees.FindAsync(id);
            if (department == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Employees.Remove(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.DepartmentID == id);
        }
    }
}
