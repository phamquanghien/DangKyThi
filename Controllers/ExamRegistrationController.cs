using System.Data.Common;
using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyCaThi.Models;
using QuanLyCaThi.Models.Process;
using System.Linq;
using QuanLyCaThi.Data;

namespace QuanLyCaThi.Controllers
{
    public class ExamRegistrationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private StringProcess _strPro = new StringProcess();

        public ExamRegistrationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ListRegisted
        public async Task<IActionResult> Index()
        {
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(ExamRegistration dkt, string subjectGroup)
        {
            if (ModelState.IsValid)
            {
                var checkFullRegisted = await _context.ExamTime.Where(m => m.SubjectID == dkt.SubjectID && m.ExamTimeID == dkt.ExamTimeID).FirstOrDefaultAsync();
                if(checkFullRegisted== null) {
                    ModelState.AddModelError("","Thông tin không hợp lệ");
                }
                else if(checkFullRegisted.IsFull == true)
                {
                    ModelState.AddModelError("","Ca thi đã đầy, không thể đăng đăng ký thêm.");
                }
                else
                {
                    dkt.FullName = _strPro.LocDau(dkt.FullName);
                    //kiem tra thong tin sinh vien co dung khong
                    var std = await _context.Student.Where(m => m.StudentCode == dkt.StudentCode && m.FullName == dkt.FullName && m.SubjectID == dkt.SubjectID && m.SubjectGroup == subjectGroup).FirstOrDefaultAsync();
                    if(std != null)
                    {
                        if(std.IsActive == true)
                        {
                            //kiem tra thong tin ca thi co dung khong
                            var examTime = await _context.ExamTime.Where(m => m.ExamTimeID == dkt.ExamTimeID && m.SubjectID == dkt.SubjectID).FirstOrDefaultAsync();
                            if(examTime != null)
                            {
                                //tat ca cac thong tin deu hop le => dang ky thi cho sinh vien
                                var registeredList = new RegisteredList();
                                registeredList.RegisteredListID = Guid.NewGuid();
                                registeredList.StudentID = std.StudentID;
                                registeredList.SubjectID = dkt.SubjectID;
                                registeredList.ExamTimeID = dkt.ExamTimeID;
                                _context.Add(registeredList);
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
                        //kiem tra sinh vien co bi cam thi khong
                        else {
                            ModelState.AddModelError("", "Sinh viên bị cấm thi, vui lòng liên hệ với GV để được hỗ trợ!");
                        }
                    }
                    //neu thong tin ma sinh vien hoac ho ten khong chinh xac
                    else {
                        var stdByCode = _context.Student.Where(m => m.StudentCode == dkt.StudentCode && m.FullName == dkt.FullName).ToList();
                        var stdByFullName = stdByCode.Where(m => m.FullName == dkt.FullName);
                        if(stdByCode.Count == 0){
                            ModelState.AddModelError("","Mã Sinh viên, họ tên Sinh viên hoặc nhóm môn học không chính xác.");
                        }else {
                            ModelState.AddModelError("","Thông tin môn thi không chính xác.");
                        }
                    }
                }
            }
            else{
                ModelState.AddModelError("", "Vui lòng chọn đầy đủ thông tin để đăng ký ca thi!");
            }
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectName", dkt.SubjectID);
            return View();
        }
        public JsonResult GetExamTimeBySubject(Guid subjectID)
        {
            var examTimeBySubject = _context.ExamTime.Where(m => m.SubjectID == subjectID);
            return Json(examTimeBySubject);
        }
        public JsonResult GetSubjectGroupByStudentID(Guid subjectID)
        {
            var subjectGroupBySubject = (from e in _context.Student
                        where e.SubjectID == subjectID
                        select e.SubjectGroup).Distinct().OrderBy(m => m).ToList();
            // var examTimeBySubject = _context.ExamTime.Where(m => m.SubjectID == subjectID);
                                
            return Json(subjectGroupBySubject);
            // return Json(new {Group =subjectGroupBySubject, time = examTimeBySubject});
        }

        // GET: ListRegisted/Create
        public IActionResult Create()
        {
            ViewData["ExamTimeID"] = new SelectList(_context.ExamTime, "ExamTimeID", "ExamTimeName");
            ViewData["StudentID"] = new SelectList(_context.Student, "StudentID", "StudentName");
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectName");
            return View();
        }

        // POST: ListRegisted/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RegisteredListID,ExamTimeID,StudentID,SubjectID")] RegisteredList registeredList)
        {
            if (ModelState.IsValid)
            {
                registeredList.RegisteredListID = Guid.NewGuid();
                _context.Add(registeredList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExamTimeID"] = new SelectList(_context.ExamTime, "ExamTimeID", "ExamTimeID", registeredList.ExamTimeID);
            ViewData["StudentID"] = new SelectList(_context.Student, "StudentID", "StudentID", registeredList.StudentID);
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectID", registeredList.SubjectID);
            return View(registeredList);
        }

        // GET: ListRegisted/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.RegisteredList == null)
            {
                return NotFound();
            }

            var registeredList = await _context.RegisteredList.FindAsync(id);
            if (registeredList == null)
            {
                return NotFound();
            }
            ViewData["ExamTimeID"] = new SelectList(_context.ExamTime, "ExamTimeID", "ExamTimeID", registeredList.ExamTimeID);
            ViewData["StudentID"] = new SelectList(_context.Student, "StudentID", "StudentID", registeredList.StudentID);
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectID", registeredList.SubjectID);
            return View(registeredList);
        }

        // POST: ListRegisted/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ListRegistedID,ExamTimeID,StudentID,SubjectID")] RegisteredList registeredList)
        {
            if (id != registeredList.RegisteredListID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registeredList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListRegistedExists(registeredList.RegisteredListID))
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
            ViewData["ExamTimeID"] = new SelectList(_context.ExamTime, "ExamTimeID", "ExamTimeID", registeredList.ExamTimeID);
            ViewData["StudentID"] = new SelectList(_context.Student, "StudentID", "StudentID", registeredList.StudentID);
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectID", registeredList.SubjectID);
            return View(registeredList);
        }

        // GET: ListRegisted/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.RegisteredList == null)
            {
                return NotFound();
            }

            var listRegisted = await _context.RegisteredList
                .Include(l => l.ExamTime)
                .Include(l => l.Student)
                .Include(l => l.Subject)
                .FirstOrDefaultAsync(m => m.RegisteredListID == id);
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
            if (_context.RegisteredList == null)
            {
                return Problem("Entity set 'ApplicationDbContext.RegisteredList'  is null.");
            }
            var registeredList = await _context.RegisteredList.FindAsync(id);
            if (registeredList != null)
            {
                _context.RegisteredList.Remove(registeredList);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ListRegistedExists(Guid id)
        {
          return (_context.RegisteredList?.Any(e => e.RegisteredListID == id)).GetValueOrDefault();
        }
    }
}
