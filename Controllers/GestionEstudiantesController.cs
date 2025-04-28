using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_VargasValeria.Data;
using ProyectoFinal_VargasValeria.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal_VargasValeria.Controllers
{
    public class GestionEstudianteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public GestionEstudianteController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // LISTAR Y BUSCAR ESTUDIANTES
        public async Task<IActionResult> Index(string searchString)
        {
            var estudiantes = _context.Estudiantes
                .Include(e => e.EstudianteCarreras)
                    .ThenInclude(ec => ec.Carrera)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                estudiantes = estudiantes.Where(e => e.Nombre.Contains(searchString) || e.Identificacion.Contains(searchString));
            }

            return View(await estudiantes.ToListAsync());
        }

        // DETALLES DEL ESTUDIANTE
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var estudiante = await _context.Estudiantes
                .Include(e => e.EstudianteCarreras)
                    .ThenInclude(ec => ec.Carrera)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (estudiante == null) return NotFound();

            return View(estudiante);
        }

        // CREAR ESTUDIANTE (GET)
        public IActionResult Create()
        {
            ViewBag.Carreras = _context.Carreras.ToList();
            return View();
        }

        // CREAR ESTUDIANTE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Identificacion,Nombre,Correo,FechaNacimiento,Telefono")] Estudiante estudiante, int[] CarreraIds)
        {
            if (ModelState.IsValid)
            {
                // Crear usuario Identity
                var user = new IdentityUser
                {
                    UserName = estudiante.Correo,
                    Email = estudiante.Correo,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, "Estudiante123!");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Estudiante");

                    // Guardar estudiante en base de datos
                    _context.Estudiantes.Add(estudiante);
                    await _context.SaveChangesAsync();

                    foreach (var carreraId in CarreraIds)
                    {
                        _context.EstudiantesCarreras.Add(new EstudianteCarrera
                        {
                            EstudianteId = estudiante.Id,
                            CarreraId = carreraId
                        });
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            ViewBag.Carreras = _context.Carreras.ToList();
            return View(estudiante);
        }

        // EDITAR ESTUDIANTE (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var estudiante = await _context.Estudiantes
                .Include(e => e.EstudianteCarreras)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (estudiante == null) return NotFound();

            ViewBag.Carreras = _context.Carreras.ToList();
            ViewBag.CarrerasSeleccionadas = estudiante.EstudianteCarreras.Select(ec => ec.CarreraId).ToArray();

            return View(estudiante);
        }

        // EDITAR ESTUDIANTE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Identificacion,Nombre,Correo,FechaNacimiento,Telefono")] Estudiante estudiante, int[] CarreraIds)
        {
            if (id != estudiante.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estudiante);
                    await _context.SaveChangesAsync();

                    // Eliminar carreras actuales
                    var carrerasActuales = _context.EstudiantesCarreras.Where(ec => ec.EstudianteId == id);
                    _context.EstudiantesCarreras.RemoveRange(carrerasActuales);
                    await _context.SaveChangesAsync();

                    // Insertar nuevas carreras
                    foreach (var carreraId in CarreraIds)
                    {
                        _context.EstudiantesCarreras.Add(new EstudianteCarrera
                        {
                            EstudianteId = id,
                            CarreraId = carreraId
                        });
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Estudiantes.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Carreras = _context.Carreras.ToList();
            return View(estudiante);
        }

        // ELIMINAR ESTUDIANTE (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var estudiante = await _context.Estudiantes
                .Include(e => e.EstudianteCarreras)
                    .ThenInclude(ec => ec.Carrera)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (estudiante == null) return NotFound();

            return View(estudiante);
        }

        // ELIMINAR ESTUDIANTE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estudiante = await _context.Estudiantes
                .Include(e => e.EstudianteCarreras)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (estudiante != null)
            {
                _context.EstudiantesCarreras.RemoveRange(estudiante.EstudianteCarreras);
                _context.Estudiantes.Remove(estudiante);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
