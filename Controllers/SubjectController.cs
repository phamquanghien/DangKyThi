using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyCaThi.Data;
using QuanLyCaThi.Models;
using QuanLyCaThi.Models.Process;

namespace QuanLyCaThi.Controllers
{
    public class SubjectController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CheckSecurityKey _checkSecurityKey;

        public SubjectController(ApplicationDbContext context, CheckSecurityKey checkSecurityKey)
        {
            _context = context;
            _checkSecurityKey = checkSecurityKey;
        }

        // GET: Subject
        public async Task<IActionResult> Index()
        {
              return _context.Subject != null ? 
                          View(await _context.Subject.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Subject'  is null.");
        }

        // GET: Subject/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Subject == null)
            {
                return NotFound();
            }

            var subject = await _context.Subject
                .FirstOrDefaultAsync(m => m.SubjectID == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subject/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Subject/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubjectID,SubjectCode,SubjectName")] Subject subject, string SecurityCode)
        {
            if(string.IsNullOrEmpty(SecurityCode))
            {
                ModelState.AddModelError("","Mã xác thực không được để trống!");
            } else {
                var checkKey = _checkSecurityKey.CheckSecurity(1,SecurityCode);
                if(checkKey==false) {
                    ModelState.AddModelError("","Mã xác thực không chính xác!");
                }
            }
            if (ModelState.IsValid)
            {
                subject.SubjectID = Guid.NewGuid();
                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        // GET: Subject/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Subject == null)
            {
                return NotFound();
            }

            var subject = await _context.Subject.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

        // POST: Subject/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("SubjectID,SubjectCode,SubjectName")] Subject subject, string SecurityCode)
        {
            if (id != subject.SubjectID)
            {
                return NotFound();
            }
            if(string.IsNullOrEmpty(SecurityCode))
            {
                ModelState.AddModelError("","Mã xác thực không được để trống!");
            } else {
                var checkKey = _checkSecurityKey.CheckSecurity(1,SecurityCode);
                if(checkKey==false) {
                    ModelState.AddModelError("","Mã xác thực không chính xác!");
                }
                else {
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(subject);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!SubjectExists(subject.SubjectID))
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
                }
            }

            
            return View(subject);
        }

        // GET: Subject/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Subject == null)
            {
                return NotFound();
            }

            var subject = await _context.Subject
                .FirstOrDefaultAsync(m => m.SubjectID == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subject/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, string SecurityCode)
        {
            if (_context.Subject == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Subject'  is null.");
            }
            var subject = await _context.Subject.FindAsync(id);
            if(string.IsNullOrEmpty(SecurityCode))
            {
                ModelState.AddModelError("","Mã xác thực không được để trống!");
            }
            else {
                var checkKey = _checkSecurityKey.CheckSecurity(1,SecurityCode);
                if(checkKey==false) {
                    ModelState.AddModelError("","Mã xác thực không chính xác!");
                } else if (subject != null)
                {
                    _context.Subject.Remove(subject);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(subject);
        }

        private bool SubjectExists(Guid id)
        {
          return (_context.Subject?.Any(e => e.SubjectID == id)).GetValueOrDefault();
        }
    }
}
