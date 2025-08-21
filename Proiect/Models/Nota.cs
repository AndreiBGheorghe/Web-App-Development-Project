using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proiect.Models
{
    public class Nota
    {
        [Key]
        public int IdNota { get; set; }
        public float Valoare { get; set; }
        public int IdStudent { get; set; }
        [ForeignKey("IdStudent")]
        public Student? Student { get; set; }
        public int IdCurs { get; set; }
        [ForeignKey("IdCurs")]
        public Curs? Curs { get; set; }
    }
}
