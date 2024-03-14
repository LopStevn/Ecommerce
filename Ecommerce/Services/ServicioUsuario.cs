using Ecommerce.Models.DATA;
using Ecommerce.Models.EN;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services
{
    public class ServicioUsuario : IServicioUsuario
    {
        private readonly EcommerceDbContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<Usuario> _signInManager;

        public ServicioUsuario(EcommerceDbContext context, UserManager<Usuario> userManager, 
            RoleManager<IdentityRole> roleManager, SignInManager<Usuario> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> ActualizarUsuario(Usuario usuario)
        {
            return await _userManager.UpdateAsync(usuario);
        }

        public async Task AsignarRol(Usuario usuario, string nombreRol)
        {
            await _userManager.AddToRoleAsync(usuario, nombreRol);
        }

        public async Task<IdentityResult> CambiarPassword(Usuario usuario, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(usuario, oldPassword, newPassword);
        }

        public async Task CerrarSesion()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> CrearUsuario(Usuario usuario, string password)
        {
            return await _userManager.CreateAsync(usuario, password);
        }

        public async Task<Usuario> CrearUsuario(UsuarioViewModel model)
        {
            Usuario usuario = new Usuario()
            {
                Email = model.Username,
                Nombre = model.Nombre,
                URLFoto = model.URLFoto,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Username,
                TipoUsuario = model.TipoUsuario
            };

            IdentityResult result = await _userManager.CreateAsync(usuario, model.Password);

            if(result != IdentityResult.Success)
            {
                return null;
            }

            Usuario nuevoUsuario = await ObtenerUsuario(model.UserName);
            await AsignarRol(nuevoUsuario, usuario.TipoUsuario.ToString());
            return nuevoUsuario;
        }

        public async Task<SignInResult> IniciarSesion(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                false);
        }

        public async Task<Usuario> ObtenerUsuario(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Usuario> ObtenerUsuario(Guid userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId.ToString());
        }

        public async Task<bool> UsuarioEnRol(Usuario usuario, string nombreRol)
        {
            return await _userManager.IsInRoleAsync(usuario, nombreRol);
        }

        public async Task VerificarRol(string nombreRol)
        {
            bool rolExiste = await _roleManager.RoleExistsAsync(nombreRol);

            if (!rolExiste)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = nombreRol
                });
            }
        }
    }
}
