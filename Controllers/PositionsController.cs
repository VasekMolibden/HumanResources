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
    public class PositionsController : Controller
    {
        private readonly HumanResourcesContext _context;

        public PositionsController(HumanResourcesContext context)
        {
            _context = context;
        }

        // GET: Positions
        public async Task<IActionResult> Index(
                    string sortOrder,
                    string currentFilter,
                    string searchString,
                    int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["SalarySortParm"] = sortOrder == "Salary" ? "salary_desc" : "Salary";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var positions = from p in _context.Positions
                           select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                positions = positions.Where(p => p.Title.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "title_desc":
                    positions = positions.OrderByDescending(p => p.Title);
                    break;
                case "Salary":
                    positions = positions.OrderBy(p => p.Salary);
                    break;
                case "salary_desc":
                    positions = positions.OrderByDescending(p => p.Salary);
                    break;
                default:
                    positions = positions.OrderBy(p => p.Title);
                    break;
            }

            int pageSize = 5;
            return View(await PaginatedList<Position>.CreateAsync(positions.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Positions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = await _context.Positions
                .FirstOrDefaultAsync(m => m.PositionID == id);
            if (position == null)
            {
                return NotFound();
            }

            return View(position);
        }

        // GET: Positions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Positions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PositionID,Title,Salary")] Position position)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(position);
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
            return View(position);
        }

        // GET: Positions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = await _context.Positions.FindAsync(id);
            if (position == null)
            {
                return NotFound();
            }
            return View(position);
        }

        // POST: Positions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PositionID,Title,Salary")] Position position)
        {
            if (id != position.PositionID)
            {
                return NotFound();
            }

            var positionToUpdate = await _context.Positions.FirstOrDefaultAsync(p => p.PositionID == id);
            if (await TryUpdateModelAsync<Position>(
                positionToUpdate,
                "",
                p => p.Title, p => p.Salary))
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
            return View(position);
        }

        // GET: Positions/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = await _context.Positions
                .FirstOrDefaultAsync(m => m.PositionID == id);
            if (position == null)
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

            return View(position);
        }

        // POST: Positions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var position = await _context.Positions.FindAsync(id);
            if (position == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Positions.Remove(position);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool PositionExists(int id)
        {
            return _context.Positions.Any(e => e.PositionID == id);
        }
    }
}
