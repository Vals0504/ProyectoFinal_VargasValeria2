using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoFinal_VargasValeria.Data;
using ProyectoFinal_VargasValeria.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal_VargasValeria.Controllers
{
    public class GestionDocenteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GestionDocenteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTAR Y BUSCAR DOCENTES
        public async Task<IActionResult> Index(string searchString)
        {
            var docentes = _context.Docentes
                                   .Include(d => d.DocenteCursos)
                                   .ThenInclude(dc => dc.Curso)
                                   .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                docentes = docentes.Where(d => d.Nombre.Contains(searchString) || d.Identificacion.Contains(searchString));
            }

            return View(await docentes.ToListAsync());
        }

        // DETALLES DEL DOCENTE
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docente = await _context.Docentes
                                        .Include(d => d.DocenteCursos)
                                        .ThenInclude(dc => dc.Curso)
                                        .FirstOrDefaultAsync(m => m.Id == id);
            if (docente == null)
            {
                return NotFound();
            }

            return View(docente);
        }

        // CREAR DOCENTE (GET)
        public IActionResult Create()
        {
            ViewData["Cursos"] = new MultiSelectList(_context.Cursos, "Id", "Nombre");
            return View();
        }

        // CREAR DOCENTE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Identificacion,Nombre,Correo,FechaNacimiento,Telefono")] Docente docente, int[] CursoIds)
        {
            if (ModelState.IsValid)
            {
                _context.Add(docente);
                await _context.SaveChangesAsync();

                foreach (var cursoId in CursoIds)
                {
                    _context.DocenteCursos.Add(new DocenteCursos
                    {
                        DocenteId = docente.Id,
                        CursoId = cursoId
                    });
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Cursos"] = new MultiSelectList(_context.Cursos, "Id", "Nombre", CursoIds);
            return View(docente);
        }

        // EDITAR DOCENTE (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docente = await _context.Docentes
                                        .Include(d => d.DocenteCursos)
                                        .FirstOrDefaultAsync(d => d.Id == id);
            if (docente == null)
            {
                return NotFound();
            }

            ViewData["Cursos"] = new MultiSelectList(_context.Cursos, "Id", "Nombre", docente.DocenteCursos.Select(dc => dc.CursoId));
            return View(docente);
        }

        // EDITAR DOCENTE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Identificacion,Nombre,Correo,FechaNacimiento,Telefono")] Docente docente, int[] CursoIds)
        {
            if (id != docente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(docente);
                    await _context.SaveChangesAsync();

                    var relacionesExistentes = _context.DocenteCursos.Where(dc => dc.DocenteId == docente.Id);
                    _context.DocenteCursos.RemoveRange(relacionesExistentes);

                    foreach (var cursoId in CursoIds)
                    {
                        _context.DocenteCursos.Add(new DocenteCursos
                        {
                            DocenteId = docente.Id,
                            CursoId = cursoId
                        });
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Docentes.Any(e => e.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Cursos"] = new MultiSelectList(_context.Cursos, "Id", "Nombre", CursoIds);
            return View(docente);
        }

        // ELIMINAR DOCENTE (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docente = await _context.Docentes
                                        .Include(d => d.DocenteCursos)
                                        .ThenInclude(dc => dc.Curso)
                                        .FirstOrDefaultAsync(m => m.Id == id);
            if (docente == null)
            {
                return NotFound();
            }
            return View(docente);
        }

        // ELIMINAR DOCENTE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var docente = await _context.Docentes.FindAsync(id);
            if (docente != null)
            {
                var relaciones = _context.DocenteCursos.Where(dc => dc.DocenteId == id);
                _context.DocenteCursos.RemoveRange(relaciones);

                _context.Docentes.Remove(docente);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
