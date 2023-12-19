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
    public class ExamTimeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CheckSecurityKey _checkSecurityKey;

        public ExamTimeController(ApplicationDbContext context, CheckSecurityKey checkSecurityKey)
        {
            _context = context;
            _checkSecurityKey = checkSecurityKey;
        }

        // GET: ExamTime
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ExamTime.Include(e => e.Subject);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ExamTime/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.ExamTime == null)
            {
                return NotFound();
            }

            var examTime = await _context.ExamTime
                .Include(e => e.Subject)
                .FirstOrDefaultAsync(m => m.ExamTimeID == id);
            if (examTime == null)
            {
                return NotFound();
            }

            return View(examTime);
        }

        // GET: ExamTime/Create
        public IActionResult Create()
        {
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectName");
            return View();
        }

        // POST: ExamTime/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExamTimeID,ExamTimeName,StartTime,FinishTime,ExamDate,MaxValue,RegistedValue,Note,IsFull,SubjectID")] ExamTime examTime, string SecurityCode)
        {
            examTime.RegistedValue = 0;
            examTime.IsFull = false;
            if(string.IsNullOrEmpty(SecurityCode))
            {
                ModelState.AddModelError("","Mã xác thực không được để trống!");
            }
            else {
                var checkKey = _checkSecurityKey.CheckSecurity(1,SecurityCode);
                if(checkKey==false) {
                    ModelState.AddModelError("","Mã xác thực không chính xác!");
                }
                else {
                    if (ModelState.IsValid)
                    {
                        examTime.ExamTimeID = Guid.NewGuid();
                        _context.Add(examTime);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectName", examTime.SubjectID);
            return View(examTime);
        }

        // GET: ExamTime/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.ExamTime == null)
            {
                return NotFound();
            }

            var examTime = await _context.ExamTime.FindAsync(id);
            if (examTime == null)
            {
                return NotFound();
            }
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectName", examTime.SubjectID);
            return View(examTime);
        }

        // POST: ExamTime/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ExamTimeID,ExamTimeName,StartTime,FinishTime,ExamDate,MaxValue,RegistedValue,Note,IsFull,SubjectID")] ExamTime examTime, string SecurityCode)
        {
            if (id != examTime.ExamTimeID)
            {
                return NotFound();
            }
            if(string.IsNullOrEmpty(SecurityCode))
            {
                ModelState.AddModelError("","Mã xác thực không được để trống!");
            }
            else {
                var checkKey = _checkSecurityKey.CheckSecurity(1,SecurityCode);
                if(checkKey==false) {
                    ModelState.AddModelError("","Mã xác thực không chính xác!");
                }
                else {
                    if(examTime.RegistedValue >= examTime.MaxValue) examTime.IsFull = true;
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(examTime);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!ExamTimeExists(examTime.ExamTimeID))
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

            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectID", examTime.SubjectID);
            return View(examTime);
        }

        // GET: ExamTime/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.ExamTime == null)
            {
                return NotFound();
            }

            var examTime = await _context.ExamTime
                .Include(e => e.Subject)
                .FirstOrDefaultAsync(m => m.ExamTimeID == id);
            if (examTime == null)
            {
                return NotFound();
            }

            return View(examTime);
        }

        // POST: ExamTime/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, string SecurityCode)
        {
            if (_context.ExamTime == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ExamTime'  is null.");
            }
            var examTime = await _context.ExamTime.FindAsync(id);
            if(string.IsNullOrEmpty(SecurityCode))
            {
                ModelState.AddModelError("","Mã xác thực không được để trống!");
            }
            else {
                var checkKey = _checkSecurityKey.CheckSecurity(1,SecurityCode);
                if(checkKey==false) {
                    ModelState.AddModelError("","Mã xác thực không chính xác!");
                } else if (examTime != null)
                {
                    _context.ExamTime.Remove(examTime);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(examTime);
        }

        private bool ExamTimeExists(Guid id)
        {
          return (_context.ExamTime?.Any(e => e.ExamTimeID == id)).GetValueOrDefault();
        }
    }
}
