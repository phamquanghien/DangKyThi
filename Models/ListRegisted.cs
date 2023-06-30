using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyCaThi.Models
{
    [Table("ListRegisteds")]
    public class ListRegisted
    {
        [Key]
        public Guid ListRegistedID { get; set; }
        public Guid ExamTimeID { get; set; }
        public Guid StudentID { get; set; }
        public Guid SubjectID { get; set; }
        [ForeignKey("SubjectID")]
        public Subject? Subject { get; set; }
        [ForeignKey("StudentID")]
        public Student? Student { get; set; }
        [ForeignKey("ExamTimeID")]
        public ExamTime? ExamTime { get; set; }
    }
}