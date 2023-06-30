using System.ComponentModel.DataAnnotations;

namespace QuanLyCaThi.Models
{
    public class DangKyThi
    {
        [Required]
        public string StudentCode { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public Guid SubjectID { get; set; }
        [Required]
        public Guid ExamTimeID { get; set; }
    }
}