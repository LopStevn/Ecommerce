using Ecommerce.Models.EN;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Services
{
    public interface IServicioUsuario
    {
        Task<Usuario> ObtenerUsuario(string email);

        Task<IdentityResult> CrearUsuario(Usuario usuario, string password);

        Task VerificarRol(string nombreRol);

        Task AsignarRol(Usuario usuario, string nombreRol);

        Task<bool> UsuarioEnRol(Usuario usuario, string nombreRol);

        Task<SignInResult> IniciarSesion(LoginViewModel model);

        Task CerrarSesion();

        Task<Usuario> CrearUsuario(UsuarioViewModel model);

        Task<IdentityResult> CambiarPassword(Usuario usuario, string oldPassword, string newPassword);

        Task<IdentityResult> ActualizarUsuario(Usuario usuario);

        Task<Usuario> ObtenerUsuario(Guid userId);
    }
}
