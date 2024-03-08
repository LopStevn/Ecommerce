using Ecommerce.Models.DATA;
using Ecommerce.Models.EN;
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

        public async Task<IActionResult> Crear()
        {
            Producto producto = new()
            {
                Categorias = await _servicioLista.GetListaCategorias(),
            };

            return View(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Producto producto, IFormFile Imagen)
        {
            if (ModelState.IsValid)
            {
                Stream imagen = Imagen.OpenReadStream();
                String UrlImagen = await _servicioImagen.SubirImagen(imagen, Imagen.FileName);

                try
                {
                    producto.URLFoto = UrlImagen;
                    _context.Add(producto);
                    await _context.SaveChangesAsync();
                    TempData["AlertMessage"] = "¡Producto creado con éxito!";
                    return RedirectToAction("Lista");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(ex.Message, "Ocurrió un error al crear el producto :(");
                }
            }
            producto.Categorias = await _servicioLista.GetListaCategorias();

            return View(producto);
        }

        public async Task<IActionResult> Editar(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);

            if(producto == null)
            {
                return NotFound();
            }

            producto.Categorias = await _servicioLista.GetListaCategorias();

            return View(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Producto producto, IFormFile Imagen)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var productoExistente = await _context.Productos.FindAsync(producto.Id);

                    if(productoExistente == null)
                    {
                        return NotFound();
                    }

                    if(Imagen != null)
                    {
                        Stream imagen = Imagen.OpenReadStream();
                        string urlImagen = await _servicioImagen.SubirImagen(imagen, Imagen.FileName);
                        productoExistente.URLFoto = urlImagen;
                    }

                    productoExistente.Nombre = producto.Nombre;
                    productoExistente.Descripcion = producto.Descripcion;
                    productoExistente.Categoria = await _context.Categorias.FindAsync(producto.CategoriaId);
                    productoExistente.Precio = producto.Precio;
                    productoExistente.Inventario = producto.Inventario;

                    _context.Update(productoExistente);
                    await _context.SaveChangesAsync();
                    TempData["AlertMessage"] = "¡El producto se actualizó con éxito!";

                    return RedirectToAction("Lista");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(ex.Message, "Ocurrió un error al actualizar el producto :(");
                }
            }

            producto.Categorias = await _servicioLista.GetListaCategorias();

            return View(producto);
        }

        public async Task<IActionResult> Eliminar(int? id)
        {
            if(id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.Id == id);

            if(producto == null)
            {
                return NotFound();
            }

            try
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
                TempData["AlertMessage"] = "¡El producto se eliminó con éxito!";
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(ex.Message, "Ocurrió un error al intentar eliminar el registro :(");
            }

            return RedirectToAction(nameof(Lista));
        }
    }
}
