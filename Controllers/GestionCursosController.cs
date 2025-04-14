using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoFinal_VargasValeria.Data;
using ProyectoFinal_VargasValeria.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal_VargasValeria.Controllers
{
    public class GestionCursosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GestionCursosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTAR Y BUSCAR CURSOS
        public async Task<IActionResult> Index(string searchString)
        {
            var cursos = _context.Cursos
                .Include(c => c.CursosCarreras)
                    .ThenInclude(cc => cc.Carrera)
                        .ThenInclude(ca => ca.CarrerasSedes)
                            .ThenInclude(cs => cs.Sede)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                cursos = cursos.Where(c => c.Nombre.Contains(searchString));
            }

            return View(await cursos.ToListAsync());
        }



        // DETALLES DEL CURSO
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos
                                      .Include(c => c.CursosCarreras)
                                      .ThenInclude(cc => cc.Carrera)
                                      .FirstOrDefaultAsync(m => m.Id == id);
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        // CREAR CURSO (GET)
        public IActionResult Create()
        {
            ViewBag.Carreras = _context.Carreras.ToList();
            return View();
        }


        // CREAR CURSO (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Creditos")] Curso curso, int[] CarreraIds)
        {
            if (ModelState.IsValid)
            {
                _context.Add(curso);
                await _context.SaveChangesAsync();

                // Asociar el curso con las carreras seleccionadas
                foreach (var carreraId in CarreraIds)
                {
                    _context.CursosCarreras.Add(new CursosCarreras
                    {
                        CursoId = curso.Id,
                        CarreraId = carreraId
                    });
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Carreras"] = new SelectList(_context.Carreras, "Id", "Nombre");
            return View(curso);
        }

        // EDITAR CURSO (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos
                                      .Include(c => c.CursosCarreras)
                                      .FirstOrDefaultAsync(c => c.Id == id);
            if (curso == null)
            {
                return NotFound();
            }

            ViewBag.Carreras = _context.Carreras.ToList();
            ViewBag.CarrerasSeleccionadas = curso.CursosCarreras.Select(cc => cc.CarreraId).ToArray();

            return View(curso);
        }


        // EDITAR CURSO (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Creditos")] Curso curso, int[] CarreraIds)
        {
            if (id != curso.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(curso);
                    await _context.SaveChangesAsync();

                    var relacionesExistentes = _context.CursosCarreras.Where(cc => cc.CursoId == curso.Id);
                    _context.CursosCarreras.RemoveRange(relacionesExistentes);

                    foreach (var carreraId in CarreraIds)
                    {
                        _context.CursosCarreras.Add(new CursosCarreras
                        {
                            CursoId = curso.Id,
                            CarreraId = carreraId
                        });
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Cursos.Any(e => e.Id == id))
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

            ViewData["Carreras"] = new MultiSelectList(_context.Carreras, "Id", "Nombre", CarreraIds);
            return View(curso);
        }

        // ELIMINAR CURSO (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos
                                      .Include(c => c.CursosCarreras)
                                      .ThenInclude(cc => cc.Carrera)
                                      .FirstOrDefaultAsync(m => m.Id == id);
            if (curso == null)
            {
                return NotFound();
            }
            return View(curso);
        }

        // ELIMINAR CURSO (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso != null)
            {
                var relaciones = _context.CursosCarreras.Where(cc => cc.CursoId == id);
                _context.CursosCarreras.RemoveRange(relaciones);

                _context.Cursos.Remove(curso);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
