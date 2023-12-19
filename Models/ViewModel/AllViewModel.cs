namespace QuanLyCaThi.Models
{
    public class AllViewModel
    {
        public Guid ListRegistedID { get; set; }
        public Guid ExamTimeID { get; set; }
        public string? ExamTimeName { get; set; }
        public string? StartTime { get; set;}
        public string? FinishTime { get; set; }
        public DateTime ExamDate { get; set; }
        public Guid StudentID { get; set; }
        public string? StudentCode { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public Guid SubjectID { get; set; }
        public string? SubjectCode { get; set; }
        public string? SubjectName { get; set; }
    }
}