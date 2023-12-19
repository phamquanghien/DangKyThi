using Microsoft.AspNetCore.Mvc;
using QuanLyCaThi.Data;

namespace QuanLyCaThi.Models.Process
{
    public class UpdateValue
    {
        private readonly ApplicationDbContext _context;

        public UpdateValue(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool UpdateValueRegisted()
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
    }
}