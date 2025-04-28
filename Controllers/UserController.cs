using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_VargasValeria.Data;
using ProyectoFinal_VargasValeria.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal_VargasValeria.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTAR Y BUSCAR CARRERAS
        public async Task<IActionResult> Carreras(string searchString)
        {
            var carreras = _context.Carreras
                                   .Include(c => c.CursosCarreras)
                                   .ThenInclude(cc => cc.Curso)
                                   .AsQueryable(); // Permite filtrar dinámicamente

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
        public async Task<IActionResult> CursosInscritos(int estudianteId)
        {
            var cursosInscritos = await _context.Matriculas
                .Where(m => m.EstudianteId == estudianteId)
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
            // Simulación de almacenamiento de mensaje o envío de correo
            TempData["MensajeEnviado"] = "¡Tu mensaje ha sido enviado exitosamente!";

            return RedirectToAction("Contacto");
        }

    }

}