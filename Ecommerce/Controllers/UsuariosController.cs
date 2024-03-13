using Ecommerce.Models.DATA;
using Ecommerce.Models.EN;
using Ecommerce.Models.ENUMS;
using Ecommerce.Models.ViewModels;
using Ecommerce.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly EcommerceDbContext _context;
        private readonly IServicioUsuario _servicioUsuario;
        private readonly IServicioLista _servicioLista;
        private readonly IServicioImagen _servicioImagen;

        public UsuariosController(EcommerceDbContext context, IServicioUsuario servicioUsuario, 
            IServicioLista servicioLista, IServicioImagen servicioImagen)
        {
            _context = context;
            _servicioUsuario = servicioUsuario;
            _servicioLista = servicioLista;
            _servicioImagen = servicioImagen;
        }

        public async Task<IActionResult> Lista()
        {
            return View(await _context.Users
                .ToListAsync());
        }

        public IActionResult Crear()
        {
            UsuarioViewModel model = new()
            {
                Id = Guid.Empty.ToString(),
                TipoUsuario = TipoUsuario.Administrador,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(UsuarioViewModel model, IFormFile Imagen)
        {
            if (ModelState.IsValid)
            {
                Stream image = Imagen.OpenReadStream();
                string URLImagen = await _servicioImagen.SubirImagen(image, Imagen.FileName);

                model.URLFoto = URLImagen;

                Usuario usuario = await _servicioUsuario.CrearUsuario(model);

                if(usuario == null)
                {
                    ModelState.AddModelError(string.Empty, "Este Email ya está siendo utilizado.");
                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}
