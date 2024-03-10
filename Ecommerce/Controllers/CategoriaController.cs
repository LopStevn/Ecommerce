using Ecommerce.Models.DATA;
using Ecommerce.Models.EN;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    [Authorize(Roles = "Administrador")]

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

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Categoria pCategoria)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(pCategoria);
                    await _context.SaveChangesAsync();
                    TempData["Alert Message"] = "¡Categoría creada con éxito!";
                    return RedirectToAction("Lista");
                }
                catch
                {
                    ModelState.AddModelError(String.Empty, "Ha ocurrido un error :(");
                }
            }

            return View(pCategoria);
        }

        public async Task<IActionResult> Editar(int? id)
        {
            if(id == null || _context.Categorias == null)
            {
                return NotFound();
            }

            var pCategoria = await _context.Categorias.FindAsync(id);

            if(pCategoria == null)
            {
                return NotFound();
            }

            return View(pCategoria);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, Categoria categoria)
        {
            if(id != categoria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                    TempData["AlertMessage"] = "¡Categoría actualizada correctamente!";
                    return RedirectToAction("Lista");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(ex.Message, "Ocurrió un error al actualizar :(");
                }
            }

            return View(categoria);
        }

        public async Task<IActionResult> Eliminar(int? id)
        {
            if(id == null || _context.Categorias == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.FirstOrDefaultAsync(n => n.Id == id);

            if(categoria == null)
            {
                return NotFound();
            }

            try
            {
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
                TempData["AlertMessage"] = "¡La categoría se eliminó correctamente!";
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(ex.Message, "Ocurrió un error, no se pudo eliminar el registro :(");
            }

            return RedirectToAction(nameof(Lista));
        }
    }
}
