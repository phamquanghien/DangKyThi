using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyCaThi.Models
{
    [Table("Students")]
    public class Student
    {
        [Key]
        public Guid StudentID { get; set; }
        [Display(Name = "Mã Sinh viên")]
        [Required(ErrorMessage = "Mã sinh viên là bắt buộc!")]
        public string StudentCode { get; set; }
        [Display(Name = "Họ")]
        [Required(ErrorMessage = "Họ là bắt buộc!")]
        public string FirstName { get; set; }
        [Display(Name = "Tên")]
        [Required(ErrorMessage = "Tên là bắt buộc!")]
        public string LastName { get; set; }
        [Display(Name = "Họ và tên")]
        public string? FullName { get; set; }
        [Display(Name = "Nhóm học phần")]
        [Required(ErrorMessage = "Nhóm môn học là bắt buộc!")]
        public string SubjectGroup { get; set; }
        [Display(Name = "Dự thi")]
        public bool IsActive { get; set; }
        public Guid SubjectID { get; set; }
        [ForeignKey("SubjectID")]
        public Subject? Subject { get; set; }
    }
}