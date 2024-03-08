using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.Services
{
    public interface IServicioLista
    {
        Task<IEnumerable<SelectListItem>> GetListaCategorias();
    }
}
