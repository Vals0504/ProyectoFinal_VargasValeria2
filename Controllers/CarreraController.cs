using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_VargasValeria.Data;
using ProyectoFinal_VargasValeria.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal_VargasValeria.Controllers
{
    public class CarreraController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarreraController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTAR Y BUSCAR CARRERAS
        public async Task<IActionResult> Index(string searchString)
        {
            var carreras = _context.Carreras
                                   .Include(c => c.CursosCarreras).ThenInclude(cc => cc.Curso)
                                   .Include(c => c.CarrerasSedes).ThenInclude(cs => cs.Sede)
                                   .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                carreras = carreras.Where(c => c.Nombre.Contains(searchString));
            }

            return View(await carreras.ToListAsync());
        }

        // DETALLES DE LA CARRERA
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var carrera = await _context.Carreras
                                        .Include(c => c.CursosCarreras).ThenInclude(cc => cc.Curso)
                                        .Include(c => c.CarrerasSedes).ThenInclude(cs => cs.Sede)
                                        .FirstOrDefaultAsync(m => m.Id == id);

            if (carrera == null) return NotFound();

            return View(carrera);
        }

        // CREAR CARRERA (GET)
        public IActionResult Create()
        {
            ViewBag.Cursos = _context.Cursos.ToList();
            ViewBag.Sedes = _context.Sedes.ToList();
            return View();
        }

        // CREAR CARRERA (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre")] Carrera carrera, int[] CursoIds, int[] SedeIds)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carrera);
                await _context.SaveChangesAsync();

                foreach (var cursoId in CursoIds)
                {
                    _context.CursosCarreras.Add(new CursosCarreras
                    {
                        CarreraId = carrera.Id,
                        CursoId = cursoId
                    });
                }

                foreach (var sedeId in SedeIds)
                {
                    _context.CarrerasSedes.Add(new CarreraSede
                    {
                        CarreraId = carrera.Id,
                        SedeId = sedeId
                    });
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Cursos = _context.Cursos.ToList();
            ViewBag.Sedes = _context.Sedes.ToList();
            return View(carrera);
        }

        // EDITAR CARRERA (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var carrera = await _context.Carreras
                .Include(c => c.CursosCarreras)
                .Include(c => c.CarrerasSedes)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (carrera == null) return NotFound();

            ViewBag.Cursos = _context.Cursos.ToList();
            ViewBag.Sedes = _context.Sedes.ToList();
            ViewBag.CursosSeleccionados = carrera.CursosCarreras.Select(cc => cc.CursoId).ToArray();
            ViewBag.SedesSeleccionadas = carrera.CarrerasSedes.Select(cs => cs.SedeId).ToArray();

            return View(carrera);
        }

        // EDITAR CARRERA (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] Carrera carrera, int[] CursoIds, int[] SedeIds)
        {
            if (id != carrera.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carrera);
                    await _context.SaveChangesAsync();

                    // Actualizar Cursos
                    var cursosRelacionados = _context.CursosCarreras.Where(cc => cc.CarreraId == carrera.Id);
                    _context.CursosCarreras.RemoveRange(cursosRelacionados);

                    foreach (var cursoId in CursoIds)
                    {
                        _context.CursosCarreras.Add(new CursosCarreras
                        {
                            CarreraId = carrera.Id,
                            CursoId = cursoId
                        });
                    }

                    // Actualizar Sedes
                    var sedesRelacionadas = _context.CarrerasSedes.Where(cs => cs.CarreraId == carrera.Id);
                    _context.CarrerasSedes.RemoveRange(sedesRelacionadas);

                    foreach (var sedeId in SedeIds)
                    {
                        _context.CarrerasSedes.Add(new CarreraSede
                        {
                            CarreraId = carrera.Id,
                            SedeId = sedeId
                        });
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Carreras.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Cursos = _context.Cursos.ToList();
            ViewBag.Sedes = _context.Sedes.ToList();
            ViewBag.CursosSeleccionados = CursoIds;
            ViewBag.SedesSeleccionadas = SedeIds;

            return View(carrera);
        }

        // ELIMINAR CARRERA (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var carrera = await _context.Carreras
                                        .Include(c => c.CursosCarreras).ThenInclude(cc => cc.Curso)
                                        .Include(c => c.CarrerasSedes).ThenInclude(cs => cs.Sede)
                                        .FirstOrDefaultAsync(c => c.Id == id);

            if (carrera == null) return NotFound();

            return View(carrera);
        }

        // ELIMINAR CARRERA (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carrera = await _context.Carreras.FindAsync(id);
            if (carrera != null)
            {
                // Eliminar relaciones
                var cursosRelacionados = _context.CursosCarreras.Where(cc => cc.CarreraId == id);
                var sedesRelacionadas = _context.CarrerasSedes.Where(cs => cs.CarreraId == id);

                _context.CursosCarreras.RemoveRange(cursosRelacionados);
                _context.CarrerasSedes.RemoveRange(sedesRelacionadas);
                _context.Carreras.Remove(carrera);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
