using System.Globalization;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using QuanLyCaThi.Data;

namespace QuanLyCaThi.Models.Process
{
    public class UpdateValue
    {
        private readonly ApplicationDbContext _context;
        StringProcess _strPro = new StringProcess();

        public UpdateValue(ApplicationDbContext context)
        {
            _context = context;
        }
        //xoa cac ban ghi khong hop le => DONE
        public bool UpdateStudentList()
        {
            bool check = false;
            var model = _context.Student.Where(m => m.StudentCode == "" || m.FirstName =="" || m.SubjectGroup == "").ToList();
            if(model.Count > 0){
                _context.RemoveRange(model);
                _context.SaveChanges();
                check = true;
            } 
            return check;
        }
        //cap nhat lai danh sach dang ky (xoa cac lan dang ky truoc chi de lai lan dang ky cuoi cung)
        public bool UpdateRegisteredList()
        {
            bool check = false;
            var listDistinct = _context.RegisteredList.Select(m => new {m.StudentID, m.SubjectID}).Distinct().ToList();
            var listRegistered = _context.RegisteredList.OrderByDescending(m => m.StudentID).ToList();
            for( int i = 0; i < listDistinct.Count - 1; i ++)
            {
                var sbID = listDistinct[i].SubjectID;
                var stdID = listDistinct[i].StudentID;
                var listDuplicate = _context.RegisteredList.OrderByDescending(m => m.StudentID).Where(m => m.SubjectID == sbID && m.StudentID == stdID).ToList();
                if(listDuplicate.Count() > 1)
                {
                    for(int j = 1; j < listDuplicate.Count; j++)
                    {
                        _context.Remove(listDuplicate[j]);
                    }
                }
            }
            check = true;
            _context.SaveChanges();
            return check;
        }
        //cap nhat so luong dang ky cua moi ca thi => DONE
        public bool UpdateValueRegistered()
        {
            bool check = false;
            var model = _context.RegisteredList.ToList();
            if(model.Count > 0){
                for(int i = 0; i < model.Count; i++)
                {
                    var extID = model[i].ExamTimeID;
                    var countRegistered = model.Where(m => m.ExamTimeID == extID).Count();
                    var examTime = _context.ExamTime.Find(extID);
                    examTime.RegistedValue = countRegistered;
                }
                check = true;
                _context.SaveChanges();
            } 
            return check;
        }
        //chuan hoa lai ten sinh vien => DONE
        public bool UpdateStudentName()
        {
            bool check = false;
            var model = _context.Student.ToList();
            if(model.Count > 0){
                for(int i = 0; i < model.Count; i++)
                {
                    model[i].FirstName = ToTitleCase(model[i].FirstName.ToLower().Trim());
                    model[i].LastName = ToTitleCase(model[i].LastName.ToLower().Trim());
                    model[i].FullName = _strPro.RemoveAccents(model[i].FirstName) + " " + _strPro.RemoveAccents(model[i].LastName);
                }
                _context.SaveChanges();
                check = true;
            } 
            return check;
        }
        //update trang thai sinh vien da dang ky ca thi => DONE
        public bool UpdateIsRegistered()
        {
            bool check = false;
            var model = _context.RegisteredList.ToList();
            if(model.Count > 0){
                for(int i = 0; i < model.Count; i++)
                {
                    var student = _context.Student.Where(m => m.SubjectID == model[i].SubjectID && m.StudentID == model[i].StudentID).First();
                    student.IsRegistered = true;
                }
                _context.SaveChanges();
                check = true;
            } 
            return check;
        }
        static string ToTitleCase(string input)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(input);
        }
    }
}