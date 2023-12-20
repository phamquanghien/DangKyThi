using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyCaThi.Models
{
    [Table("ExamTimes")]
    public class ExamTime
    {
        [Key]
        public Guid ExamTimeID { get; set; }
        public string ExamTimeName { get; set; }
        public string StartTime { get; set;}
        public string FinishTime { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExamDate { get; set; }
        public int MaxValue { get; set; }
        public int RegistedValue { get; set; }
        public string? Note { get; set; }
        public bool IsFull { get; set; }
        public Guid SubjectID { get; set; }
        [ForeignKey("SubjectID")]
        public Subject? Subject { get; set; }
    }
}