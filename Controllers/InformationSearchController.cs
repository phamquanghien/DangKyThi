using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyCaThi.Data;

namespace QuanLyCaThi.Controllers
{
    public class InformationSearchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InformationSearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ExamTime
        public async Task<IActionResult> Index()
        {
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string StudentCode, Guid SubjectID)
        {
            var query = (from registed in _context.RegisteredList
                        join std in _context.Student on registed.StudentID equals std.StudentID
                        join subject in _context.Subject on registed.SubjectID equals subject.SubjectID
                        join time in _context.ExamTime on registed.ExamTimeID equals time.ExamTimeID
                        where std.StudentCode == StudentCode && subject.SubjectID == SubjectID
                        select new {
                            StdCode = std.StudentCode,
                            stdFName = std.FirstName + " " + std.LastName,
                            Subject = subject.SubjectName,
                            Cathi = time.ExamTimeName
                        }).ToList();
            if(query.Count == 1){
                string message = "Sinh viên " + query[0].stdFName + " (" + query[0].StdCode +") ";
                message += " đã đăng ký thành công ca thi: " + query[0].Cathi;
                message += " môn " + query[0].Subject;
                ViewBag.Result = message;
            } else {
                ViewBag.Result = "Sinh viên chưa đăng ký ca thi";
            }
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectName");
            return View();
        }
    }
}