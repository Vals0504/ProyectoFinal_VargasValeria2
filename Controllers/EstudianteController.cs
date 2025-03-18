using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_VargasValeria.Data;
using ProyectoFinal_VargasValeria.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal_VargasValeria.Controllers
{
    public class EstudianteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EstudianteController(ApplicationDbContext context)
        {
            _context = context;
        }
        // LISTAR Y BUSCAR CARRERAS
        public async Task<IActionResult> Index(string searchString)
        {
            var carreras = _context.Carreras.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                carreras = carreras.Where(c => c.Nombre.Contains(searchString));
            }

            return View(await carreras.ToListAsync());
        }

        // Método para ver las carreras disponibles
        public async Task<IActionResult> Carreras()
        {
            var carreras = await _context.Carreras.Include(c => c.CursosCarreras).ThenInclude(cc => cc.Curso).ToListAsync();
            return View(carreras);
        }
    }
}
