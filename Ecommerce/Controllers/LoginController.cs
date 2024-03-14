using Ecommerce.Models.DATA;
using Ecommerce.Models.EN;
using Ecommerce.Models.ENUMS;
using Ecommerce.Models.ViewModels;
using Ecommerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    public class LoginController : Controller
    {
        private readonly IServicioUsuario _servicioUsuario;
        private readonly EcommerceDbContext _context;
        private readonly IServicioLista _servicioLista;
        private readonly IServicioImagen _servicioImagen;

        public LoginController(IServicioUsuario servicioUsuario, EcommerceDbContext context,
            IServicioLista servicioLista, IServicioImagen servicioImagen)
        {
            _servicioUsuario = servicioUsuario;
            _context = context;
            _servicioLista = servicioLista;
            _servicioImagen = servicioImagen;
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

        public IActionResult Registro()
        {
            UsuarioViewModel model = new()
            {
                Id = Guid.Empty.ToString(),
                TipoUsuario = TipoUsuario.Cliente,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(UsuarioViewModel model, IFormFile Imagen)
        {
            if (ModelState.IsValid)
            {
                Stream image = Imagen.OpenReadStream();
                string URLImagen = await _servicioImagen.SubirImagen(image, Imagen.FileName);

                model.URLFoto = URLImagen;

                Usuario usuario = await _servicioUsuario.CrearUsuario(model);

                if(usuario == null)
                {
                    ModelState.AddModelError(string.Empty, "Este correo ya esta siendo utilizado.");
                    return View(model);
                }

                LoginViewModel loginViewModel = new()
                {
                    Password = model.Password,
                    RememberMe = false,
                    Username = model.Username
                };

                var result = await _servicioUsuario.IniciarSesion(loginViewModel);

                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> EditarUsuario()
        {
            Usuario usuario = await _servicioUsuario.ObtenerUsuario(User.Identity.Name);

            if(usuario == null)
            {
                return NotFound();
            }

            Usuario model = new()
            {
                Nombre = usuario.Nombre,
                PhoneNumber = usuario.PhoneNumber,
                URLFoto = usuario.URLFoto,
                Id = usuario.Id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarUsuario(Usuario model, IFormFile Imagen)
        {
            if (ModelState.IsValid)
            {
                Usuario usuario = await _servicioUsuario.ObtenerUsuario(User.Identity.Name);
                usuario.Nombre = model.Nombre;
                usuario.PhoneNumber = model.PhoneNumber;

                Stream image = Imagen.OpenReadStream();
                string URLImagen = await _servicioImagen.SubirImagen(image, Imagen.FileName);

                usuario.URLFoto = URLImagen;

                await _servicioUsuario.ActualizarUsuario(usuario);
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public IActionResult CambiarPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CambiarPassword(PasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _servicioUsuario.ObtenerUsuario(User.Identity.Name);
                
                if(usuario != null)
                {
                    var result = await _servicioUsuario.CambiarPassword(usuario, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("EditarUsuario");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuario no encontrado.");
                }

            }
            return View(model);
        }

    }
}
