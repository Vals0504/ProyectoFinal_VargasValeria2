using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinal_VargasValeria.Controllers
{
    [Authorize(Roles = "User")]
    public class EstudianteDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
