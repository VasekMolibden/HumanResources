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
    public class EmployeesController : Controller
    {
        private readonly HumanResourcesContext _context;

        public EmployeesController(HumanResourcesContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index(
                    string sortOrder,
                    string currentFilter,
                    string searchString,
                    int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["DOBSortParm"] = sortOrder == "DOB" ? "dob_desc" : "DOB";
            ViewData["PositionSortParm"] = sortOrder == "position" ? "position_desc" : "position";
            ViewData["DepartmentSortParm"] = sortOrder == "department" ? "department_desc" : "department";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var employees = from e in _context.Employees.Include(p => p.Position).Include(d => d.Department)
                            select e;
            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e => e.Name.Contains(searchString)
                || e.Position.Title.Contains(searchString)
                 || e.Department.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    employees = employees.OrderByDescending(e => e.Name);
                    break;
                case "Date":
                    employees = employees.OrderBy(e => e.DateOfEmployment);
                    break;
                case "date_desc":
                    employees = employees.OrderByDescending(e => e.DateOfEmployment);
                    break;
                case "DOB":
                    employees = employees.OrderBy(e => e.DateOfBirth);
                    break;
                case "dob_desc":
                    employees = employees.OrderByDescending(e => e.DateOfBirth);
                    break;
                case "position":
                    employees = employees.OrderBy(e => e.Position.Title);
                    break;
                case "position_desc":
                    employees = employees.OrderByDescending(e => e.Position.Title);
                    break;
                case "department":
                    employees = employees.OrderBy(e => e.Department.Name);
                    break;
                case "department_desc":
                    employees = employees.OrderByDescending(e => e.Department.Name);
                    break;
                default:
                    employees = employees.OrderBy(e => e.Name);
                    break;
            }

            int pageSize = 5;
            return View(await PaginatedList<Employee>.CreateAsync(employees.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name");
            ViewData["PositionID"] = new SelectList(_context.Positions, "PositionID", "Title");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,DateOfEmployment,PhoneNumber,DateOfBirth,PositionID,DepartmentID")] Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(employee);
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
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name", employee.DepartmentID);
            ViewData["PositionID"] = new SelectList(_context.Positions, "PositionID", "Title", employee.PositionID);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name", employee.DepartmentID);
            ViewData["PositionID"] = new SelectList(_context.Positions, "PositionID", "Title", employee.PositionID);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
       public async Task<IActionResult> EditPost(int? id, [Bind("ID,Name,DateOfEmployment,PhoneNumber,DateOfBirth,PositionID,DepartmentID")] Employee employee)
        {
            if (id == null)
            {
                return NotFound();
            }
            var employeeToUpdate = await _context.Employees.FirstOrDefaultAsync(e => e.ID == id);
            if (await TryUpdateModelAsync<Employee>(
                employeeToUpdate,
                "",
                e => e.Name, e => e.DateOfEmployment, e => e.PhoneNumber, e => e.DateOfBirth, e => e.PositionID, e => e.DepartmentID))
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
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name", employee.DepartmentID);
            ViewData["PositionID"] = new SelectList(_context.Positions, "PositionID", "Title", employee.PositionID);
            return View(employeeToUpdate);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .AsNoTracking()
                .Include(e => e.Department)
                .Include(e => e.Position)
                .FirstOrDefaultAsync(m => m.ID == id);
                
            if (employee == null)
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

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.ID == id);
        }
    }
}
