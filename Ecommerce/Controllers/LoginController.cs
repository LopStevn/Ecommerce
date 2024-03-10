using Ecommerce.Models.DATA;
using Ecommerce.Models.ViewModels;
using Ecommerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    public class LoginController : Controller
    {
        private readonly IServicioUsuario _servicioUsuario;
        private readonly EcommerceDbContext _context;

        public LoginController(IServicioUsuario servicioUsuario, EcommerceDbContext context)
        {
            _servicioUsuario = servicioUsuario;
            _context = context;
        }

        public IActionResult IniciarSesion()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _servicioUsuario.IniciarSesion(model);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos");
            }
            return View(model);
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await _servicioUsuario.CerrarSesion();
            return RedirectToAction("Index", "Home");
        }
    }
}
