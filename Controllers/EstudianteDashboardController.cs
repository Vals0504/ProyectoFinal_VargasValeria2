﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinal_VargasValeria.Controllers
{
    [Authorize(Roles = "Estudiante")]
    public class EstudianteDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
