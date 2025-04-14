using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ProyectoFinal_VargasValeria.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult AlumnoLogin()
        {
            return View();
        }

        public IActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home"); 
        }

        [HttpPost]
        public async Task<IActionResult> LoginAlumno(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "El correo y la contraseña son obligatorios.");
                return View("AlumnoLogin");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !await _userManager.IsInRoleAsync(user, "User"))
            {
                ModelState.AddModelError("", "Correo incorrecto o usuario no autorizado.");
                return View("AlumnoLogin");
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Contraseña incorrecta. Intente de nuevo.");
                return View("AlumnoLogin");
            }

            return RedirectToAction("Index", "EstudianteDashboard");
        }

        [HttpPost]
        public async Task<IActionResult> LoginAdmin(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "El correo y la contraseña son obligatorios.");
                return View("AdminLogin");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !await _userManager.IsInRoleAsync(user, "Admin"))
            {
                ModelState.AddModelError("", "Correo incorrecto o usuario no autorizado.");
                return View("AdminLogin");
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Contraseña incorrecta. Intente de nuevo.");
                return View("AdminLogin");
            }

            return RedirectToAction("Index", "Admin");
        }

    }
}
