using Proiect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Drawing;
using Rotativa.AspNetCore;

namespace Proiect.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ILogger<CatalogController> _logger;
        private readonly CatalogContext _context;
        public CatalogController(ILogger<CatalogController> logger, CatalogContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            var utilizator = HttpContext.Session.GetString("User");
            if (!string.IsNullOrEmpty(utilizator))
            {
                return RedirectToAction("Principal");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Login(Utilizator model)
        {
            var utilizator = _context.Utilizatori
                .FirstOrDefault(u => u.NumeUtilizator == model.NumeUtilizator && u.Parola == model.Parola);
            if (utilizator != null)
            {
                HttpContext.Session.SetInt32("Rol", utilizator.Rol);
                if (utilizator.Rol == 1)
                {
                    var student = _context.Studenti
                        .FirstOrDefault(s => (s.NumeStudent + " " + s.PrenumeStudent) == utilizator.NumeUtilizator);
                    if (student != null)
                    {
                        HttpContext.Session.SetString("User", student.NumeStudent + " " + student.PrenumeStudent);
                        HttpContext.Session.SetInt32("IdStudent", student.IdStudent);
                    }
                    else
                    {
                        HttpContext.Session.SetString("User", utilizator.NumeUtilizator);
                    }
                }
                else if (utilizator.Rol == 2)
                {
                    var profesor = _context.Profesori
                        .FirstOrDefault(p => (p.NumeProfesor + " " + p.PrenumeProfesor) == utilizator.NumeUtilizator);
                    if (profesor != null)
                    {
                        HttpContext.Session
                            .SetString("User", profesor.NumeProfesor + " " + profesor.PrenumeProfesor);
                    }
                    else
                    {
                        HttpContext.Session
                            .SetString("User", utilizator.NumeUtilizator);
                    }
                }
                else
                {
                    HttpContext.Session.SetString("User", utilizator.NumeUtilizator);
                }
                return RedirectToAction("Principal");
            }
            ViewBag.Error = "Nume sau parola incorecte";
            return View("Index", model);
        }
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        public IActionResult Principal()
        {
            var utilizator = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(utilizator))
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult NoteStudent(string sortOrder, string search)
        {
            var rol = HttpContext.Session.GetInt32("Rol");
            if (rol != 1)
                return RedirectToAction("Index");
            var numeComplet = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(numeComplet))
                return RedirectToAction("Index");
            var parti = numeComplet.Split(' ');
            if (parti.Length < 2)
                return RedirectToAction("Index");
            var nume = parti[0];
            var prenume = parti[1];
            var student = _context.Studenti
                .FirstOrDefault(s => s.NumeStudent == nume && s.PrenumeStudent == prenume);
            if (student == null)
                return RedirectToAction("Index");
            var note = _context.Note.Include(n => n.Curs).ThenInclude(c => c.Profesor)
                .Where(n => n.IdStudent == student.IdStudent).ToList();
            if (!string.IsNullOrEmpty(search))
            {
                var searchLower = search.ToLower();
                note = note.Where(n => n.Curs.NumeCurs.ToLower()
                .Contains(searchLower) || (n.Curs.Profesor.NumeProfesor + " " + n.Curs.Profesor.PrenumeProfesor)
                .ToLower().Contains(searchLower)).ToList();
            }
            if (sortOrder == "nume_desc")
                note = note.OrderByDescending(n => n.Curs.NumeCurs).ToList();
            else if (sortOrder == "nota_asc")
                note = note.OrderBy(n => n.Valoare).ToList();
            else if (sortOrder == "nota_desc")
                note = note.OrderByDescending(n => n.Valoare).ToList();
            else
                note = note.OrderBy(n => n.Curs.NumeCurs).ToList();
            return View(note);
        }
        public IActionResult CursuriProfesor()
        {
            var rol = HttpContext.Session.GetInt32("Rol");
            if (rol != 2)
                return RedirectToAction("Index");
            var numeProfesorComplet = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(numeProfesorComplet))
                return RedirectToAction("Index");
            var parts = numeProfesorComplet.Split(' ');
            if (parts.Length < 2)
                return RedirectToAction("Index");
            var profesor = _context.Profesori
                .FirstOrDefault(p => p.NumeProfesor == parts[0] && p.PrenumeProfesor == parts[1]);
            if (profesor == null)
                return RedirectToAction("Index");
            var cursuri = _context.Cursuri.Where(c => c.IdProfesor == profesor.IdProfesor).ToList();
            var note = _context.Note.Include(n => n.Student).Where(n => cursuri.Select(c => c.IdCurs)
                .Contains(n.IdCurs)).ToList();
            var notePeCurs = note.GroupBy(n => n.IdCurs).ToDictionary(g => g.Key, g => g.ToList());
            ViewData["NotePeCurs"] = notePeCurs;
            return View(cursuri);
        }
        [HttpPost]
        public IActionResult SalveazaNote(Dictionary<int, float> note)
        {
            foreach (var entry in note)
            {
                var nota = _context.Note.FirstOrDefault(n => n.IdNota == entry.Key);
                if (nota != null)
                {
                    nota.Valoare = entry.Value;
                    if (entry.Value > 0)
                    {
                        var alerta = new Alerta
                        {
                            IdStudent = nota.IdStudent,
                            Continut = $"Ai primit o nota noua ({entry.Value}).",
                            Citita = false
                        };
                        _context.Alerte.Add(alerta);
                    }
                }
            }
            _context.SaveChanges();
            return RedirectToAction("CursuriProfesor");
        }
        public IActionResult CataloageSecretar(string search)
        {
            var rol = HttpContext.Session.GetInt32("Rol");
            if (rol != 3)
                return RedirectToAction("Index");
            var noteQuery = _context.Note.Include(n => n.Curs).ThenInclude(c => c.Profesor)
                .Include(n => n.Student).AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                var s = search.ToLower();
                noteQuery = noteQuery.Where(n => n.Curs.NumeCurs.ToLower()
                    .Contains(s) || (n.Curs.Profesor.NumeProfesor + " " + n.Curs.Profesor.PrenumeProfesor)
                    .ToLower().Contains(s) || (n.Student.NumeStudent + " " + n.Student.PrenumeStudent)
                    .ToLower().Contains(s));
            }
            var noteList = noteQuery.OrderBy(n => n.Curs.An).ThenBy(n => n.Curs.NumeCurs)
                .ThenBy(n => n.Student.NumeStudent).ToList();
            return View(noteList);
        }
        public IActionResult ExportPdf()
        {
            var catalog = _context.Note.Include(n => n.Curs).ThenInclude(c => c.Profesor)
                .Include(n => n.Student).ToList();
            return new ViewAsPdf("CataloagePdf", catalog)
            {
                FileName = "catalog.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                CustomSwitches = "--enable-local-file-access"
            };
        }
        public IActionResult Moderare()
        {
            if (HttpContext.Session.GetInt32("Rol") != 4)
                return RedirectToAction("Index");
            ViewData["Profesori"] = _context.Profesori.ToList();
            ViewData["Cursuri"] = _context.Cursuri.Include(c => c.Profesor).ToList();
            ViewData["Studenti"] = _context.Studenti.ToList();
            return View(_context.Note.Include(n => n.Curs).Include(n => n.Student).ToList());
        }
        [HttpPost]
        public IActionResult CreeazaCurs(string numeCurs, int an, int idProfesor)
        {
            if (HttpContext.Session.GetInt32("Rol") != 4)
                return RedirectToAction("Index");
            var curs = new Curs { NumeCurs = numeCurs, An = an, IdProfesor = idProfesor };
            _context.Cursuri.Add(curs);
            _context.SaveChanges();
            return RedirectToAction("Moderare");
        }
        [HttpPost]
        public IActionResult ModificaCurs(int idCurs, string numeCurs, int an, int idProfesor)
        {
            if (HttpContext.Session.GetInt32("Rol") != 4)
                return RedirectToAction("Index");
            var curs = _context.Cursuri.Find(idCurs);
            if (curs != null)
            { 
                curs.NumeCurs = numeCurs;
                curs.An = an;
                curs.IdProfesor = idProfesor;
                _context.SaveChanges();
            }
            return RedirectToAction("Moderare");
        }
        [HttpPost]
        public IActionResult StergeCurs(int idCurs)
        {
            if (HttpContext.Session.GetInt32("Rol") != 4)
                return RedirectToAction("Index");
            var curs = _context.Cursuri.Find(idCurs);
            if (curs != null)
            {
                var note = _context.Note.Where(n => n.IdCurs == idCurs).ToList();
                _context.Note.RemoveRange(note);
                _context.Cursuri.Remove(curs);
                _context.SaveChanges();
            }
            return RedirectToAction("Moderare");
        }
        [HttpPost]
        public IActionResult AdaugaStudentLaCurs(int idCurs, int idStudent)
        {
            if (HttpContext.Session.GetInt32("Rol") != 4)
                return RedirectToAction("Index");
            bool exista = _context.Note.Any(n => n.IdCurs == idCurs && n.IdStudent == idStudent);
            if (!exista)
            {
                var notaNoua = new Nota { IdCurs = idCurs, IdStudent = idStudent, Valoare = 0 };
                _context.Note.Add(notaNoua);
                var curs = _context.Cursuri.FirstOrDefault(c => c.IdCurs == idCurs);
                var alerta = new Alerta
                {
                    IdStudent = idStudent,
                    Continut = $"Ai fost adaugat la cursul '{curs?.NumeCurs ?? "necunoscut"}'.",
                    Citita = false
                };
                _context.Alerte.Add(alerta);
                _context.SaveChanges();
            }
            return RedirectToAction("Moderare");
        }
        [HttpPost]
        public IActionResult AdaugaProfesor(int idCurs, int idProfesor)
        {
            if (HttpContext.Session.GetInt32("Rol") != 4)
                return RedirectToAction("Index");
            var curs = _context.Cursuri.Find(idCurs);
            if (curs != null && curs.IdProfesor != idProfesor)
            {
                curs.IdProfesor = idProfesor;
                _context.SaveChanges();
            }
            return RedirectToAction("Moderare");
        }
        public IActionResult AdeverintaPdf()
        {
            var rol = HttpContext.Session.GetInt32("Rol");
            if (rol != 1)
                return RedirectToAction("Index");
            var numeComplet = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(numeComplet))
                return RedirectToAction("Index");
            var parti = numeComplet.Split(' ');
            if (parti.Length < 2)
                return RedirectToAction("Index");
            var nume = parti[0];
            var prenume = parti[1];
            var student = _context.Studenti.FirstOrDefault(s => s.NumeStudent == nume && s.PrenumeStudent == prenume);
            if (student == null)
                return RedirectToAction("Index");
            return new ViewAsPdf("AdeverintaPdf", student)
            {
                FileName = "adeverinta.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                CustomSwitches = "--enable-local-file-access"
            };
        }
        public IActionResult MesajePrimite()
        {
            var rol = HttpContext.Session.GetInt32("Rol");
            if (rol != 2)
                return RedirectToAction("Index");
            var numeProfesorComplet = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(numeProfesorComplet))
                return RedirectToAction("Index");
            var parts = numeProfesorComplet.Split(' ');
            if (parts.Length < 2)
                return RedirectToAction("Index");
            var profesor = _context.Profesori
                .FirstOrDefault(p => p.NumeProfesor == parts[0] && p.PrenumeProfesor == parts[1]);
            if (profesor == null)
                return RedirectToAction("Index");
            var mesaje = _context.Mesaje.Where(m => m.IdProfesor == profesor.IdProfesor)
                .OrderByDescending(m => m.DataTrimiterii).ToList();
            return View(mesaje);
        }
        [HttpGet]
        public IActionResult TrimiteMesaj()
        {
            if (HttpContext.Session.GetInt32("Rol") != 3)
                return RedirectToAction("Index");
            ViewData["Profesori"] = _context.Profesori.ToList();
            var mesaje = _context.Mesaje.ToList();
            return View(mesaje);
        }
        [HttpPost]
        public IActionResult TrimiteMesaj(int idProfesor, string continut)
        {
            if (HttpContext.Session.GetInt32("Rol") != 3)
                return RedirectToAction("Index");
            var mesaj = new Mesaj
            {
                IdProfesor = idProfesor,
                Continut = continut,
                NumeSecretar = HttpContext.Session.GetString("User") ?? "Secretar",
                DataTrimiterii = DateTime.Now
            };
            _context.Mesaje.Add(mesaj);
            _context.SaveChanges();
            return RedirectToAction("TrimiteMesaj");
        }
        public IActionResult AlerteStudent()
        {
            int? idStudent = HttpContext.Session.GetInt32("IdStudent");
            if (idStudent == null)
                return RedirectToAction("Index");
            var alerte = _context.Alerte.Where(a => a.IdStudent == idStudent).OrderByDescending(a => a.IdAlerta).ToList();
            foreach (var alerta in alerte.Where(a => !a.Citita))
            {
                alerta.Citita = true;
            }
            _context.SaveChanges();
            return View(alerte);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}