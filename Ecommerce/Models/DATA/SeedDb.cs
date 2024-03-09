using Ecommerce.Models.EN;
using Ecommerce.Models.ENUMS;
using Ecommerce.Services;

namespace Ecommerce.Models.DATA
{
    public class SeedDb
    {
        private readonly EcommerceDbContext _context;
        private readonly IServicioUsuario _servicioUsuario;

        public SeedDb(EcommerceDbContext context, IServicioUsuario servicioUsuario)
        {
            _context = context;
            _servicioUsuario = servicioUsuario;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await VerificarCategoriasAsync();
            await VerificarRolesAsync();
            await VerificarUsuarioAsync("Administrador", "administrador@gmail.com", "7171 6767",
                TipoUsuario.Administrador);
        }

        private async Task<Usuario> VerificarUsuarioAsync(string nombre, string email, string telefono, TipoUsuario tipoUsuario)
        {
            Usuario usuario = await _servicioUsuario.ObtenerUsuario(email);
             
            if(usuario == null)
            {
                usuario = new Usuario
                {
                    Nombre = nombre,
                    Email = email,
                    UserName = email,
                    PhoneNumber = telefono,
                    TipoUsuario = tipoUsuario
                };

                await _servicioUsuario.CrearUsuario(usuario, "123456");
                await _servicioUsuario.AsignarRol(usuario, tipoUsuario.ToString());
            }

            return usuario;
        }

        private async Task VerificarRolesAsync()
        {
            await _servicioUsuario.VerificarRol(TipoUsuario.Administrador.ToString());
            await _servicioUsuario.VerificarRol(TipoUsuario.Cliente.ToString());
        }

        private async Task VerificarCategoriasAsync()
        {
            if (!_context.Categorias.Any())
            {
                _context.Categorias.Add(new Categoria { Nombre = "Disney" });
                _context.Categorias.Add(new Categoria { Nombre = "Animales" });
                _context.Categorias.Add(new Categoria { Nombre = "Independencia" });
                _context.Categorias.Add(new Categoria { Nombre = "Navideños" });
            }

            await _context.SaveChangesAsync();
        }
    }
}
