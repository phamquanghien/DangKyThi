using System.ComponentModel.DataAnnotations;

namespace QuanLyCaThi.Models
{
    public class SecurityCode
    {
        [Key]
        public int id { get; set; }
        public string SecurityKey { get; set; }
    }
}