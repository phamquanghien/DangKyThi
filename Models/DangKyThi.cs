using System.ComponentModel.DataAnnotations;

namespace QuanLyCaThi.Models
{
    public class DangKyThi
    {
        [Required(ErrorMessage = "Vui lòng nhập mã sinh viên!")]
        public string StudentCode { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập họ tên sinh viên!")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn môn học!")]
        public Guid SubjectID { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn ca thi!")]
        public Guid ExamTimeID { get; set; }
    }
}