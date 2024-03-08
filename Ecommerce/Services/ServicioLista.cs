using Ecommerce.Models.DATA;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services
{
    public class ServicioLista : IServicioLista
    {
        private readonly EcommerceDbContext _context;

        public ServicioLista(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SelectListItem>> GetListaCategorias()
        {
            List<SelectListItem> lista = await _context.Categorias.Select(x => new SelectListItem
            {
                Text = x.Nombre,
                Value = $"{x.Id}"
            })
                .OrderBy(x => x.Text)
                .ToListAsync();

            lista.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una categoria]",
                Value = "0"
            });

            return lista;
        }
    }
}
