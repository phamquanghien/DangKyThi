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
    public class ListRegistedController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CheckSecurityKey _checkSecurityKey;
        private StringProcess _strPro = new StringProcess();
        public ListRegistedController(ApplicationDbContext context, CheckSecurityKey checkSecurityKey)
        {
            _context = context;
            _checkSecurityKey = checkSecurityKey;
        }

        // GET: ListRegisted
        public async Task<IActionResult> Index()
        {
            var model = _context.ListRegisted.Include(l => l.ExamTime).Include(l => l.Student).Include(l => l.Subject);
            ViewBag.countStudent = model.ToList().Count();
            return View(await model.ToListAsync());
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
        // public IActionResult Create()
        // {
        //     ViewData["ExamTimeID"] = new SelectList(_context.ExamTime, "ExamTimeID", "ExamTimeName");
        //     ViewData["StudentID"] = new SelectList(_context.Student, "StudentID", "FullName");
        //     ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectName");
        //     return View();
        // }

        // // POST: ListRegisted/Create
        // // To protect from overposting attacks, enable the specific properties you want to bind to.
        // // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([Bind("ListRegistedID,ExamTimeID,StudentID,SubjectID")] ListRegisted listRegisted)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         listRegisted.ListRegistedID = Guid.NewGuid();
        //         _context.Add(listRegisted);
        //         await _context.SaveChangesAsync();
        //         return RedirectToAction(nameof(Index));
        //     }
        //     ViewData["ExamTimeID"] = new SelectList(_context.ExamTime, "ExamTimeID", "ExamTimeName", listRegisted.ExamTimeID);
        //     ViewData["StudentID"] = new SelectList(_context.Student, "StudentID", "FullName", listRegisted.StudentID);
        //     ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectName", listRegisted.SubjectID);
            
        //     return View(listRegisted);
        // }
        public async Task<IActionResult> Create()
        {
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DangKyThi dkt, string SecurityCode)
        {
            if(string.IsNullOrEmpty(SecurityCode)) {
                ModelState.AddModelError("","Mã xác thực không được để trống!");
            } else {
                var checkKey = _checkSecurityKey.CheckSecurity(1,SecurityCode);
                if(checkKey==false) {
                    ModelState.AddModelError("","Mã xác thực không chính xác!");
                } else {
                    if (ModelState.IsValid) {
                        dkt.FullName = _strPro.LocDau(dkt.FullName);
                        //kiem tra thong tin sinh vien co dung khong
                        var std = await _context.Student.Where(m => m.StudentCode == dkt.StudentCode && m.FullName == dkt.FullName && m.SubjectID == dkt.SubjectID).FirstOrDefaultAsync();
                        if(std != null) {
                            //kiem tra thong tin ca thi co dung khong
                            var examTime = await _context.ExamTime.Where(m => m.ExamTimeID == dkt.ExamTimeID && m.SubjectID == dkt.SubjectID).FirstOrDefaultAsync();
                            if(examTime != null)
                            {
                                //tat ca cac thong tin deu hop le => dang ky thi cho sinh vien
                                var registed = new ListRegisted();
                                registed.ListRegistedID = Guid.NewGuid();
                                registed.StudentID = std.StudentID;
                                registed.SubjectID = dkt.SubjectID;
                                registed.ExamTimeID = dkt.ExamTimeID;
                                _context.Add(registed);
                                //cap nhat thong tin so luong sinh vien da dang ky ca thi
                                var exTime = await _context.ExamTime.FindAsync(dkt.ExamTimeID);
                                examTime.RegistedValue = exTime.RegistedValue + 1;
                                if(examTime.RegistedValue >= examTime.MaxValue) examTime.IsFull = true;
                                //luu thong tin vao database
                                await _context.SaveChangesAsync();
                                ViewBag.infoSuccess = "Sinh viên " + dkt.FullName + " (" + dkt.StudentCode + ") đăng ký ca thi thành công!";
                                //return RedirectToAction(nameof(Index));
                            } else {
                                ModelState.AddModelError("","Ca thi không hợp lệ. Vui lòng liên hệ với GV để được hỗ trợ.");
                            }
                        }
                        //neu thong tin ma sinh vien hoac ho ten khong chinh xac
                        else {
                            var stdByCode = _context.Student.Where(m => m.StudentCode == dkt.StudentCode && m.FullName == dkt.FullName).ToList();
                            var stdByFullName = stdByCode.Where(m => m.FullName == dkt.FullName);
                            if(stdByCode.Count == 0){
                                ModelState.AddModelError("","Mã Sinh viên hoặc họ tên Sinh viên không chính xác.");
                            }else {
                                ModelState.AddModelError("","Thông tin môn thi không chính xác.");
                            }
                        }
                    }
                    else{
                        ModelState.AddModelError("", "Vui lòng chọn đầy đủ thông tin để đăng ký ca thi!");
                    }
                }
            }
            
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectName", dkt.SubjectID);
            return View();
        }
        public JsonResult GetExamTimeBySubject(Guid subjectID)
        {
            var examTimeBySubject = _context.ExamTime.Where(m => m.SubjectID == subjectID);
            return Json(examTimeBySubject);
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
            var subjectRegisted = await _context.Subject.FindAsync(listRegisted.SubjectID);
            ViewBag.SubjectName = subjectRegisted.SubjectName;
            var studentRegisted = await _context.Student.FindAsync(listRegisted.StudentID);
            ViewBag.FullName = studentRegisted.FirstName + " " + studentRegisted.LastName + " (" + studentRegisted.StudentCode + ")";
            var cathi = _context.ExamTime.Where(m => m.SubjectID == listRegisted.SubjectID);
            ViewData["ExamTimeID"] = new SelectList(cathi, "ExamTimeID", "ExamTimeName", listRegisted.ExamTimeID);
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
