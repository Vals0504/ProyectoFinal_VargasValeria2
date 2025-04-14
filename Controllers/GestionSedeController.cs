using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_VargasValeria.Data;
using ProyectoFinal_VargasValeria.Models;
using System.Threading.Tasks;
using System.Linq;

namespace ProyectoFinal_VargasValeria.Controllers
{
    public class GestionSedeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GestionSedeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTAR
        public async Task<IActionResult> Index(string searchString)
        {
            var sedes = _context.Sedes.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                sedes = sedes.Where(s => s.Nombre.Contains(searchString));
            }

            return View(await sedes.ToListAsync());
        }


        // CREAR (GET)
        public IActionResult Create()
        {
            return View();
        }

        // CREAR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre")] Sede sede)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sede);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sede);
        }

        // EDITAR (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var sede = await _context.Sedes.FindAsync(id);
            if (sede == null) return NotFound();

            return View(sede);
        }

        // EDITAR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] Sede sede)
        {
            if (id != sede.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(sede);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(sede);
        }

        // ELIMINAR (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var sede = await _context.Sedes.FirstOrDefaultAsync(m => m.Id == id);
            if (sede == null) return NotFound();

            return View(sede);
        }

        // ELIMINAR (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var sede = await _context.Sedes.FindAsync(id);
            if (sede != null)
            {
                _context.Sedes.Remove(sede);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}