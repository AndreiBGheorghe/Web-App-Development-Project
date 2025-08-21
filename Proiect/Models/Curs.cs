using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proiect.Models
{
    public class Curs
    {
        [Key]
        public int IdCurs { get; set; }
        public string NumeCurs { get; set; }
        public int An { get; set; }
        public int IdProfesor { get; set; }
        [ForeignKey("IdProfesor")]
        public Profesor? Profesor { get; set; }

    }
}
