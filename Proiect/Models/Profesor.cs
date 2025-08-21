using System.ComponentModel.DataAnnotations;

namespace Proiect.Models
{
    public class Profesor
    {
        [Key]
        public int IdProfesor { get; set; }
        public string NumeProfesor { get; set; }
        public string PrenumeProfesor { get; set; }
    }
}
