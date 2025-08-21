using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Proiect.Models
{
    public class Alerta
    {
        [Key]
        public int IdAlerta { get; set; }
        public int IdStudent { get; set; }
        [ForeignKey("IdStudent")]
        public Student? Student { get; set; }
        public string Continut { get; set; }
        public bool Citita { get; set; } = false;
    }
}