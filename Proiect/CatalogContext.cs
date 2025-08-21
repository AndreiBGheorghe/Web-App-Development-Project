using Proiect.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace Proiect
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options) { }
        public DbSet<Curs> Cursuri { get; set; }
        public DbSet<Student> Studenti { get; set; }
        public DbSet<Nota> Note { get; set; }
        public DbSet<Profesor> Profesori { get; set; }
        public DbSet<Utilizator> Utilizatori { get; set; }
        public DbSet<Mesaj> Mesaje { get; set; }
        public DbSet<Alerta> Alerte { get; set; }
    }
}
