using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyCaThi.Data;
using QuanLyCaThi.Models;
using QuanLyCaThi.Models.Process;

namespace QuanLyCaThi.Controllers
{
    public class RegisteredListController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CheckSecurityKey _checkSecurityKey;
        private StringProcess _strPro = new StringProcess();
        public RegisteredListController(ApplicationDbContext context, CheckSecurityKey checkSecurityKey)
        {
            _context = context;
            _checkSecurityKey = checkSecurityKey;
        }

        // GET: RegisteredList
        public async Task<IActionResult> Index()
        {
            var model = _context.RegisteredList.Include(l => l.ExamTime).Include(l => l.Student).Include(l => l.Subject);
            ViewBag.countStudent = model.ToList().Count();
            return View(await model.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Index(string? keySearch)
        {
            if(!string.IsNullOrEmpty(keySearch) && keySearch != "") {
                keySearch = _strPro.LocDau(keySearch);
            }
            var model = _context.RegisteredList.Include(l => l.ExamTime).Include(l => l.Student).Include(l => l.Subject).Where(m => m.Student.StudentCode.Contains(keySearch) || m.Student.FullName.Contains(keySearch));
            ViewBag.countStudent = model.ToList().Count();
            return View(await model.ToListAsync());
        }
        public async Task<IActionResult> Create()
        {
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ExamRegistration dkt, string SecurityCode)
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
                                var registered = new RegisteredList();
                                registered.RegisteredListID = Guid.NewGuid();
                                registered.StudentID = std.StudentID;
                                registered.SubjectID = dkt.SubjectID;
                                registered.ExamTimeID = dkt.ExamTimeID;
                                _context.Add(registered);
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

        // GET: RegisteredList/Edit/5
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
            var subjectRegistered = await _context.Subject.FindAsync(registeredList.SubjectID);
            ViewBag.SubjectName = subjectRegistered.SubjectName;
            var studentRegisted = await _context.Student.FindAsync(registeredList.StudentID);
            ViewBag.FullName = studentRegisted.FirstName + " " + studentRegisted.LastName + " (" + studentRegisted.StudentCode + ")";
            var cathi = _context.ExamTime.Where(m => m.SubjectID == registeredList.SubjectID);
            ViewData["ExamTimeID"] = new SelectList(cathi, "ExamTimeID", "ExamTimeName", registeredList.ExamTimeID);
            return View(registeredList);
        }

        // POST: ListRegisted/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("RegisteredListID,ExamTimeID,StudentID,SubjectID")] RegisteredList registeredList)
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

        // POST: RegisteredList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.RegisteredList == null)
            {
                return Problem("Entity set 'ApplicationDbContext.RegisteredList'  is null.");
            }
            var listRegisted = await _context.RegisteredList.FindAsync(id);
            if (listRegisted != null)
            {
                _context.RegisteredList.Remove(listRegisted);
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
