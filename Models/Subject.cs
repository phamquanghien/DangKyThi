using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyCaThi.Models
{
    [Table("Subjects")]
    public class Subject
    {
        [Key]
        public Guid SubjectID { get; set; }
        [Display(Name = "Mã Học phần")]
        public string SubjectCode { get; set; }
        [Display(Name = "Tên Học phần")]
        public string SubjectName { get; set; }
    }
}