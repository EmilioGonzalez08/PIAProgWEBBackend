using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIAProgWEB.Models.dbModels
{
    [Table("Dirección_envio")]
    public partial class DirecciónEnvio
    {
        [Key]
        [Column("DirecciónID")]
        public int DirecciónId { get; set; }
        public string DirecciónCompleta { get; set; } = null!;
        [Column("UsuarioID")]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        [InverseProperty("DirecciónEnvios")]
        public virtual ApplicationUser Usuario { get; set; } = null!;
    }
}
