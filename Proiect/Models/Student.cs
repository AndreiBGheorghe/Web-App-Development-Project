using System.ComponentModel.DataAnnotations;

namespace Proiect.Models
{
    public class Student
    {
        [Key]
        public int IdStudent { get; set; }
        public string NumeStudent { get; set; }
        public string PrenumeStudent { get; set; }
        public int Grupa { get; set; }
    }
}
