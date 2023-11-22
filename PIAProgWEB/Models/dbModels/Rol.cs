using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIAProgWEB.Models.dbModels
{
    [Table("Rol")]
    public partial class Rol
    {
        public Rol()
        {
            Usuarios = new HashSet<ApplicationUser>();
        }

        [Key]
        [Column("RolID")]
        public int RolId { get; set; }
        [Column("Rol")]
        [StringLength(50)]
        public string Rol1 { get; set; } = null!;

        [InverseProperty("Rol")]
        public virtual ICollection<ApplicationUser> Usuarios { get; set; }
    }
}
