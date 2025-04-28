using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_VargasValeria.Data;
using ProyectoFinal_VargasValeria.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoFinal_VargasValeria.Controllers
{
    public class EstudianteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EstudianteController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // LISTAR Y BUSCAR CARRERAS
        public async Task<IActionResult> Carreras(string searchString)
        {
            var carreras = _context.Carreras
                                   .Include(c => c.CursosCarreras)
                                       .ThenInclude(cc => cc.Curso)
                                   .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                carreras = carreras.Where(c => c.Nombre.Contains(searchString));
            }

            return View(await carreras.ToListAsync());
        }

        public IActionResult SobreNosotros()
        {
            return View();
        }

        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> CursosInscritos()
        {
            var user = await _userManager.GetUserAsync(User);
            var estudiante = await _context.Estudiantes.FirstOrDefaultAsync(e => e.Correo == user.Email);

            if (estudiante == null)
                return RedirectToAction("SobreNosotros");

            var cursosInscritos = await _context.Matriculas
                .Where(m => m.EstudianteId == estudiante.Id)
                .Include(m => m.Curso)
                .Select(m => m.Curso)
                .ToListAsync();

            return View(cursosInscritos);
        }



        public IActionResult Contacto()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EnviarMensaje(string Mensaje, string Nombre, string Apellido, string Email)
        {
            TempData["MensajeEnviado"] = "¡Tu mensaje ha sido enviado exitosamente!";
            return RedirectToAction("Contacto");
        }

        // --- MATRÍCULA ---
        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> Matricula()
        {
            var user = await _userManager.GetUserAsync(User);
            var estudiante = await _context.Estudiantes.FirstOrDefaultAsync(e => e.Correo == user.Email);

            if (estudiante == null)
            {
                return RedirectToAction("SobreNosotros");
            }

            // Obtener las carreras del estudiante
            var carreraIds = await _context.EstudiantesCarreras
                .Where(ec => ec.EstudianteId == estudiante.Id)
                .Select(ec => ec.CarreraId)
                .ToListAsync();

            // Obtener los cursos disponibles en sus carreras
            var cursos = await _context.CursosCarreras
                .Where(cc => carreraIds.Contains(cc.CarreraId))
                .Select(cc => cc.Curso)
                .Distinct()
                .ToListAsync();

            return View(cursos);
        }

        // POST: Matricularse a cursos
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> RealizarMatricula(int[] CursoIds)
        {
            var user = await _userManager.GetUserAsync(User);
            var estudiante = await _context.Estudiantes.FirstOrDefaultAsync(e => e.Correo == user.Email);

            if (estudiante == null)
            {
                return RedirectToAction("SobreNosotros");
            }

            foreach (var cursoId in CursoIds)
            {
                var yaMatriculado = await _context.Matriculas
                    .AnyAsync(m => m.EstudianteId == estudiante.Id && m.CursoId == cursoId);

                if (!yaMatriculado)
                {
                    _context.Matriculas.Add(new Matricula
                    {
                        EstudianteId = estudiante.Id,
                        CursoId = cursoId,
                        FechaMatricula = DateTime.Now, 
                        Estado = EstadoMatricula.Activa 
                    });
                }
            }

            await _context.SaveChangesAsync();

            TempData["MensajeMatricula"] = "¡Cursos matriculados correctamente!";
            return RedirectToAction("CursosInscritos");
        }

    }
}
