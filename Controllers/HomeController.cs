using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal_VargasValeria.Models;

namespace ProyectoFinal_VargasValeria.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string role)
        {
            if (role == "Alumno")
            {
                return RedirectToAction("AlumnoLogin", "Account");
            }
            else if (role == "Administrador")
            {
                return RedirectToAction("AdminLogin", "Account");
            }

            return RedirectToAction("Index");
        }
    }
}

