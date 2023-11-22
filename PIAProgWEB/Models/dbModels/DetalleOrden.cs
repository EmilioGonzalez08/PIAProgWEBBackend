using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIAProgWEB.Models.dbModels
{
    [Table("Detalle_orden")]
    public partial class DetalleOrden
    {
        [Key]
        [Column("OrdenID")]
        public int OrdenId { get; set; }
        [Key]
        [Column("ProductoID")]
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        [Column(TypeName = "money")]
        public decimal PrecioUnitario { get; set; }

        [ForeignKey("OrdenId")]
        [InverseProperty("DetalleOrdens")]
        public virtual Orden Orden { get; set; } = null!;
        [ForeignKey("ProductoId")]
        [InverseProperty("DetalleOrdens")]
        public virtual Producto Producto { get; set; } = null!;
    }
}
