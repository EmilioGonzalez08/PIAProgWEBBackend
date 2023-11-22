using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIAProgWEB.Models.dbModels
{
    [Table("Novedad")]
    public partial class Novedad
    {
        public Novedad()
        {
            ImagenNovedads = new HashSet<ImagenNovedad>();
        }

        [Key]
        public int IdNovedad { get; set; }
        public string Descripcion { get; set; } = null!;
        public int IdEstacion { get; set; }

        [ForeignKey("IdEstacion")]
        [InverseProperty("Novedads")]
        public virtual Estacione IdEstacionNavigation { get; set; } = null!;
        [InverseProperty("IdNovedadNavigation")]
        public virtual ICollection<ImagenNovedad> ImagenNovedads { get; set; }
    }
}
