using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyCaThi.Models
{
    [Table("Students")]
    public class Student
    {
        [Key]
        public Guid StudentID { get; set; }
        public string StudentCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string SubjectGroup { get; set; }
        public bool IsActive { get; set; }
        public Guid SubjectID { get; set; }
        [ForeignKey("SubjectID")]
        public Subject? Subject { get; set; }
    }
}