using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PIAProgWEB.Models.dbModels
{
    public class ApplicationUser : IdentityUser<int>
    {
        public ApplicationUser()
        {
            Carritos = new HashSet<Carrito>();
            DirecciónEnvios = new HashSet<DirecciónEnvio>();
            Ordens = new HashSet<Orden>();
            IdProductos = new HashSet<Producto>();
        }

        [InverseProperty("Usuarios")]
        public virtual Rol Rol { get; set; } = null!;
        [InverseProperty("Usuario")]
        public virtual ICollection<Carrito> Carritos { get; set; }
        [InverseProperty("Usuario")]
        public virtual ICollection<DirecciónEnvio> DirecciónEnvios { get; set; }
        [InverseProperty("Usuario")]
        public virtual ICollection<Orden> Ordens { get; set; }

        [ForeignKey("IdUsuario")]
        [InverseProperty("IdUsuarios")]
        public virtual ICollection<Producto> IdProductos { get; set; }
    }
}
