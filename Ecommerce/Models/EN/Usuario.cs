using Ecommerce.Models.ENUMS;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models.EN
{
    public class Usuario : IdentityUser
    {
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener como máximo {1} caractéres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Nombre { get; set; }

        [Display(Name = "Foto")]
        public string URLFoto { get; set; }

        public TipoUsuario TipoUsuario { get; set; }
    }
}
