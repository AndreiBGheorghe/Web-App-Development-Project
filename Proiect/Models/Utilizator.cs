using System.ComponentModel.DataAnnotations;

namespace Proiect.Models
{
    public class Utilizator
    {
        [Key]
        public int IdUtilizator { get; set; }
        public string NumeUtilizator { get; set; }
        public string Parola { get; set; }
        public int Rol { get; set; } // 1 - Student, 2 - Profesor, 3 - Secretar, 4 - Moderator
    }
}
