using Ecommerce.Models.DATA;
using Ecommerce.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    public class ProductoController : Controller
    {
        private readonly EcommerceDbContext _context;
        private readonly IServicioImagen _servicioImagen;
        private readonly IServicioLista _servicioLista;

        public ProductoController(EcommerceDbContext context, IServicioImagen servicioImagen, IServicioLista servicioLista)
        {
            _context = context;
            _servicioImagen = servicioImagen;
            _servicioLista = servicioLista;
        }

        public async Task<IActionResult> Lista()
        {
            return View(await _context.Productos
                .Include(p => p.Categoria)
                .ToListAsync());
        }
    }
}
