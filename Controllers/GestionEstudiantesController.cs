using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoFinal_VargasValeria.Data;
using ProyectoFinal_VargasValeria.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal_VargasValeria.Controllers
{
    public class GestionEstudianteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GestionEstudianteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTAR Y BUSCAR ESTUDIANTES
        public async Task<IActionResult> Index(string searchString)
        {
            var estudiantes = _context.Estudiantes.Include(e => e.Carrera).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                estudiantes = estudiantes.Where(e => e.Nombre.Contains(searchString) || e.Identificacion.Contains(searchString));
            }

            return View(await estudiantes.ToListAsync());
        }

        // DETALLES DEL ESTUDIANTE
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiantes
                                           .Include(e => e.Carrera)
                                           .FirstOrDefaultAsync(m => m.Id == id);
            if (estudiante == null)
            {
                return NotFound();
            }

            return View(estudiante);
        }

        // CREAR ESTUDIANTE (GET)
        public IActionResult Create()
        {
            ViewData["CarreraId"] = new SelectList(_context.Carreras, "Id", "Nombre");
            return View();
        }

        // CREAR ESTUDIANTE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Identificacion,Nombre,Correo,FechaNacimiento,Telefono,CarreraId")] Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estudiante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CarreraId"] = new SelectList(_context.Carreras, "Id", "Nombre", estudiante.CarreraId);
            return View(estudiante);
        }

        // EDITAR ESTUDIANTE (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null)
            {
                return NotFound();
            }

            ViewData["CarreraId"] = new SelectList(_context.Carreras, "Id", "Nombre", estudiante.CarreraId);
            return View(estudiante);
        }

        // EDITAR ESTUDIANTE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Identificacion,Nombre,Correo,FechaNacimiento,Telefono,CarreraId")] Estudiante estudiante)
        {
            if (id != estudiante.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estudiante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Estudiantes.Any(e => e.Id == id))
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

            ViewData["CarreraId"] = new SelectList(_context.Carreras, "Id", "Nombre", estudiante.CarreraId);
            return View(estudiante);
        }

        // ELIMINAR ESTUDIANTE (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiantes
                                           .Include(e => e.Carrera)
                                           .FirstOrDefaultAsync(m => m.Id == id);
            if (estudiante == null)
            {
                return NotFound();
            }
            return View(estudiante);
        }

        // ELIMINAR ESTUDIANTE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante != null)
            {
                _context.Estudiantes.Remove(estudiante);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
