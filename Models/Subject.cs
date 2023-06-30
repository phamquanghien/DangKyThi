using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyCaThi.Models
{
    [Table("Subjects")]
    public class Subject
    {
        [Key]
        public Guid SubjectID { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
    }
}