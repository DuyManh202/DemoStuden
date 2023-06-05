using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemoStuden.Data;
using DemoStuden.Models;

namespace DemoStuden.Controllers
{
    public class StudensController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudensController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Studens
        public IActionResult Index(string search, string filter, int page = 1)
        {
            IQueryable<Studens> students = _context.Studens;

            if (!string.IsNullOrEmpty(search))
            {
                students = students.Where(s => s.Name.Contains(search));
            }

            if (!string.IsNullOrEmpty(filter))
            {
                students = students.Where(s => s.Class == filter);
            }

            int pageSize = 10;
            int totalStudents = students.Count();
            int totalPages = (int)Math.Ceiling(totalStudents / (double)pageSize);

            students = students.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.SearchString = search;
            ViewBag.Filter = filter;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(students.ToList());
        }



        // GET: Studens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Studens == null)
            {
                return NotFound();
            }

            var studens = await _context.Studens
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studens == null)
            {
                return NotFound();
            }

            return View(studens);
        }

        // GET: Studens/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Studens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Age,Class")] Studens studens)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studens);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(studens);
        }

        // GET: Studens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Studens == null)
            {
                return NotFound();
            }

            var studens = await _context.Studens.FindAsync(id);
            if (studens == null)
            {
                return NotFound();
            }
            return View(studens);
        }

        // POST: Studens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Age,Class")] Studens studens)
        {
            if (id != studens.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studens);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudensExists(studens.Id))
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
            return View(studens);
        }

        // GET: Studens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Studens == null)
            {
                return NotFound();
            }

            var studens = await _context.Studens
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studens == null)
            {
                return NotFound();
            }

            return View(studens);
        }

        // POST: Studens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Studens == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Studens'  is null.");
            }
            var studens = await _context.Studens.FindAsync(id);
            if (studens != null)
            {
                _context.Studens.Remove(studens);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudensExists(int id)
        {
          return (_context.Studens?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
