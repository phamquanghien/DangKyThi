using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using QuanLyCaThi.Data;
using QuanLyCaThi.Models.Process;

namespace QuanLyCaThi.Controllers
{
    public class DownloadController : Controller
    {
        private readonly ApplicationDbContext _context;
        private StringProcess _strPro = new StringProcess();
        public DownloadController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectName");
            return View();
        }
        public IActionResult getRegisteredList(Guid? SubjectID, Guid? ExamTimeID )
        {
            var fileName = "YourFile";
            var query = (from registered in _context.RegisteredList
                            join std in _context.Student on registered.StudentID equals std.StudentID
                            join sb in _context.Subject on registered.SubjectID equals sb.SubjectID
                            join ext in _context.ExamTime on registered.ExamTimeID equals ext.ExamTimeID
                            select new
                            {
                                registered.SubjectID,
                                registered.ExamTimeID,
                                std.StudentCode,
                                std.FirstName,
                                std.LastName,
                                std.SubjectGroup,
                                ext.ExamTimeName,
                                sb.SubjectCode,
                                sb.SubjectName
                            }).ToList();
            if(SubjectID!=null){
                query = query.Where(m => m.SubjectID == SubjectID).ToList();
                if(query.Count > 0)
                {
                    fileName = query.First().SubjectCode + "_" + query.First().SubjectName;
                    fileName = _strPro.RemoveAccents(fileName.Trim());
                }
                
            }
            if(ExamTimeID != null) {
                query = query.Where(m => m.ExamTimeID == ExamTimeID).ToList();
                fileName += "_" + query.First().ExamTimeName;
            }
            var result = query.Select(m => new {
                m.StudentCode,
                m.FirstName,
                m.LastName,
                m.SubjectName,
                m.SubjectGroup,
                m.ExamTimeName
            });
            fileName += ".xlsx";
            using(ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");
                excelWorksheet.Cells["A1"].Value = "Mã Sinh viên";
                excelWorksheet.Cells["B1"].Value = "Họ";
                excelWorksheet.Cells["C1"].Value = "Tên";
                excelWorksheet.Cells["D1"].Value = "Môn thi";
                excelWorksheet.Cells["E1"].Value = "Nhóm";
                excelWorksheet.Cells["F1"].Value = "Ca thi";
                excelWorksheet.Cells["A2"].LoadFromCollection(result);
                var stream = new MemoryStream(excelPackage.GetAsByteArray());
                return File(stream,"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",fileName);
            }
        }
        public IActionResult getUnRegisteredList(Guid? SubjectID)
        {
            var fileName = "YourFile";
            var query = (from std in _context.Student
                            join sb in _context.Subject on std.SubjectID equals sb.SubjectID
                            where std.IsRegistered == false
                            select new
                            {
                                std.StudentCode,
                                std.FirstName,
                                std.LastName,
                                std.SubjectGroup,
                                sb.SubjectID,
                                sb.SubjectCode,
                                sb.SubjectName
                            }).ToList();
            if(SubjectID!=null){
                query = query.Where(m => m.SubjectID == SubjectID).ToList();
                if(query.Count > 0)
                {
                    fileName = query.First().SubjectCode + "_" + query.First().SubjectName;
                    fileName = _strPro.RemoveAccents(fileName.Trim());
                }
                
            }
            var result = query.Select(m => new {
                m.StudentCode,
                m.FirstName,
                m.LastName,
                m.SubjectGroup,
                m.SubjectCode,
                m.SubjectName
            });
            fileName += ".xlsx";
            using(ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");
                excelWorksheet.Cells["A1"].Value = "Mã Sinh viên";
                excelWorksheet.Cells["B1"].Value = "Họ";
                excelWorksheet.Cells["C1"].Value = "Tên";
                excelWorksheet.Cells["D1"].Value = "Nhóm";
                excelWorksheet.Cells["E1"].Value = "Mã môn học";
                excelWorksheet.Cells["F1"].Value = "Tên môn học";
                excelWorksheet.Cells["A2"].LoadFromCollection(result);
                var stream = new MemoryStream(excelPackage.GetAsByteArray());
                return File(stream,"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",fileName);
            }
        }
    }
}