using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyCaThi.Data;
using QuanLyCaThi.Models;

namespace QuanLyCaThi.Controllers
{
    public class ListRegistedController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ListRegistedController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ListRegisted
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ListRegisted.Include(l => l.ExamTime).Include(l => l.Student).Include(l => l.Subject);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ListRegisted/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.ListRegisted == null)
            {
                return NotFound();
            }

            var listRegisted = await _context.ListRegisted
                .Include(l => l.ExamTime)
                .Include(l => l.Student)
                .Include(l => l.Subject)
                .FirstOrDefaultAsync(m => m.ListRegistedID == id);
            if (listRegisted == null)
            {
                return NotFound();
            }

            return View(listRegisted);
        }

        // GET: ListRegisted/Create
        public IActionResult Create()
        {
            ViewData["ExamTimeID"] = new SelectList(_context.ExamTime, "ExamTimeID", "ExamTimeID");
            ViewData["StudentID"] = new SelectList(_context.Student, "StudentID", "StudentID");
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectID");
            return View();
        }

        // POST: ListRegisted/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ListRegistedID,ExamTimeID,StudentID,SubjectID")] ListRegisted listRegisted)
        {
            if (ModelState.IsValid)
            {
                listRegisted.ListRegistedID = Guid.NewGuid();
                _context.Add(listRegisted);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExamTimeID"] = new SelectList(_context.ExamTime, "ExamTimeID", "ExamTimeID", listRegisted.ExamTimeID);
            ViewData["StudentID"] = new SelectList(_context.Student, "StudentID", "StudentID", listRegisted.StudentID);
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectID", listRegisted.SubjectID);
            return View(listRegisted);
        }

        // GET: ListRegisted/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.ListRegisted == null)
            {
                return NotFound();
            }

            var listRegisted = await _context.ListRegisted.FindAsync(id);
            if (listRegisted == null)
            {
                return NotFound();
            }
            ViewData["ExamTimeID"] = new SelectList(_context.ExamTime, "ExamTimeID", "ExamTimeID", listRegisted.ExamTimeID);
            ViewData["StudentID"] = new SelectList(_context.Student, "StudentID", "StudentID", listRegisted.StudentID);
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectID", listRegisted.SubjectID);
            return View(listRegisted);
        }

        // POST: ListRegisted/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ListRegistedID,ExamTimeID,StudentID,SubjectID")] ListRegisted listRegisted)
        {
            if (id != listRegisted.ListRegistedID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(listRegisted);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListRegistedExists(listRegisted.ListRegistedID))
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
            ViewData["ExamTimeID"] = new SelectList(_context.ExamTime, "ExamTimeID", "ExamTimeID", listRegisted.ExamTimeID);
            ViewData["StudentID"] = new SelectList(_context.Student, "StudentID", "StudentID", listRegisted.StudentID);
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectID", listRegisted.SubjectID);
            return View(listRegisted);
        }

        // GET: ListRegisted/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.ListRegisted == null)
            {
                return NotFound();
            }

            var listRegisted = await _context.ListRegisted
                .Include(l => l.ExamTime)
                .Include(l => l.Student)
                .Include(l => l.Subject)
                .FirstOrDefaultAsync(m => m.ListRegistedID == id);
            if (listRegisted == null)
            {
                return NotFound();
            }

            return View(listRegisted);
        }

        // POST: ListRegisted/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.ListRegisted == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ListRegisted'  is null.");
            }
            var listRegisted = await _context.ListRegisted.FindAsync(id);
            if (listRegisted != null)
            {
                _context.ListRegisted.Remove(listRegisted);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ListRegistedExists(Guid id)
        {
          return (_context.ListRegisted?.Any(e => e.ListRegistedID == id)).GetValueOrDefault();
        }
    }
}
