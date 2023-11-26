using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIAProgWEB.Models.dbModels
{
    [Table("ProductoTalla")]
    public partial class ProductoTalla
    {
        [Key]
        [Column("TallaID")]
        public int TallaId { get; set; }
        [Key]
        [Column("ProductoID")]
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }


        [ForeignKey("ProductoId")]
        [InverseProperty("ProductoTallas")]
        public virtual Producto Producto { get; set; } = null!;
        [ForeignKey("TallaId")]
        [InverseProperty("ProductoTallas")]
        public virtual Talla Talla { get; set; } = null!;

    }
}
