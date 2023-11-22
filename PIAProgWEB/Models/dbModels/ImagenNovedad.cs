using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIAProgWEB.Models.dbModels
{
    [Table("Imagen_novedad")]
    public partial class ImagenNovedad
    {
        [Key]
        public int IdImagen { get; set; }
        public int IdNovedad { get; set; }
        public string Imagen { get; set; } = null!;

        [ForeignKey("IdNovedad")]
        [InverseProperty("ImagenNovedads")]
        public virtual Novedad IdNovedadNavigation { get; set; } = null!;
    }
}
