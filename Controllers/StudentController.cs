using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyCaThi.Models;
using QuanLyCaThi.Models.Process;

namespace QuanLyCaThi.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private ExcelProcess _excelProcess = new ExcelProcess();
        private StringProcess _strPro = new StringProcess();

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Student
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Student.Include(s => s.Subject);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Student == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Subject)
                .FirstOrDefaultAsync(m => m.StudentID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectName");
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentID,StudentCode,FirstName,LastName,FullName,BirthDay,SubjectGroup,IsActive,SubjectID")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.StudentID = Guid.NewGuid();
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectName", student.SubjectID);
            return View(student);
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Student == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectName", student.SubjectID);
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("StudentID,StudentCode,FirstName,LastName,FullName,BirthDay,SubjectGroup,IsActive,SubjectID")] Student student)
        {
            if (id != student.StudentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentID))
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
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectName", student.SubjectID);
            return View(student);
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Student == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Subject)
                .FirstOrDefaultAsync(m => m.StudentID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Student == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Student'  is null.");
            }
            var student = await _context.Student.FindAsync(id);
            if (student != null)
            {
                _context.Student.Remove(student);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Upload()
        {
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file, string SecurityCode, Guid subjectId)
        {
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectName");
            if(file != null)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                if(fileExtension != ".xls" && fileExtension != ".xlsx")
                {
                    ModelState.AddModelError("", "Please choose excel file to upload!");
                }
                else if(SecurityCode != "14022004")
                {
                    ModelState.AddModelError("", "Khoá bảo mật không đúng. Vui lòng thử lại");
                }
                else
                {
                    var fileName = DateTime.Now.ToShortTimeString() + fileExtension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory() + "/Uploads/Excels", fileName);
                    var fileLocation = new FileInfo(filePath).ToString();
                    using(var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        var dt =  _excelProcess.ExcelToDataTable(fileLocation);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            try
                            {
                                var std = new Student();
                                std.StudentCode = dt.Rows[i][0].ToString();
                                std.FirstName = dt.Rows[i][1].ToString();
                                std.LastName = dt.Rows[i][2].ToString();
                                std.FullName = _strPro.LocDau(std.FirstName.Trim()) + " " + _strPro.LocDau(std.LastName.Trim());
                                std.SubjectGroup = dt.Rows[i][3].ToString();
                                if(dt.Rows[i][4].ToString() == "0") std.IsActive = false;
                                else std.IsActive = true;
                                std.SubjectID = subjectId;
                                _context.Student.Add(std);
                            }catch(Exception ex)
                            {
                                ModelState.AddModelError("","File excel không đúng định dạng");
                                return View();
                            }
                            
                        }
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            ModelState.AddModelError("","Vui lòng chọn file Excel để upload.");
            return View();
        }
        private bool StudentExists(Guid id)
        {
          return (_context.Student?.Any(e => e.StudentID == id)).GetValueOrDefault();
        }
    }
}
