using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proiect.Models
{
    public class Mesaj
    {
        [Key]
        public int IdMesaj { get; set; }
        public int IdProfesor { get; set; }
        [ForeignKey("IdProfesor")]
        public Profesor? Profesor { get; set; }
        public string NumeSecretar { get; set; }
        public string Continut { get; set; }
        public DateTime DataTrimiterii { get; set; }
    }
}