using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIAProgWEB.Models.dbModels
{
    [Table("Carrito")]
    public partial class Carrito
    {
        [Key, Column("UsuarioID", Order = 0)]
        public int UsuarioId { get; set; }

        [Key, Column("ProductioID", Order = 1)]
        public int ProductioId { get; set; }

        public int Cantidad { get; set; }
        public decimal Subtotal => Cantidad * Productio.Precio;

        [Column(TypeName = "date")]
        public DateTime Fecha { get; set; }

        [ForeignKey("ProductioId")]
        [InverseProperty("Carritos")]
        public virtual Producto Productio { get; set; } = null!;

        [ForeignKey("UsuarioId")]
        [InverseProperty("Carritos")]
        public virtual ApplicationUser Usuario { get; set; } = null!;
    }
}
    
