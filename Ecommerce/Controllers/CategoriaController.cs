using Ecommerce.Models.DATA;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly EcommerceDbContext _context;

        public CategoriaController(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Lista()
        {
            return View(await _context.Categorias.ToListAsync());
        }
    }
}
