using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIAProgWEB.Models.dbModels
{
    [Table("Producto")]
    public partial class Producto
    {
        public Producto()
        {
            Carritos = new HashSet<Carrito>();
            DetalleOrdens = new HashSet<DetalleOrden>();
            ProductoTallas = new HashSet<ProductoTalla>();
            IdUsuarios = new HashSet<ApplicationUser>();
        }

        [Key]
        [Column("ProductoID")]
        public int ProductoId { get; set; }
        [StringLength(50)]
        public string NombreProducto { get; set; } = null!;
        public string Descripción { get; set; } = null!;
        [Column(TypeName = "money")]
        public decimal Precio { get; set; }
        [Column("CategoriaID")]
        public int CategoriaId { get; set; }
        [StringLength(255)]
        public string Imagen { get; set; } = null!;

        [ForeignKey("CategoriaId")]
        [InverseProperty("Productos")]
        public virtual Categorium Categoria { get; set; } = null!;
        [InverseProperty("Productio")]
        public virtual ICollection<Carrito> Carritos { get; set; }
        [InverseProperty("Producto")]
        public virtual ICollection<DetalleOrden> DetalleOrdens { get; set; }
        [InverseProperty("Producto")]
        public virtual ICollection<ProductoTalla> ProductoTallas { get; set; }

        [ForeignKey("IdProducto")]
        [InverseProperty("IdProductos")]
        public virtual ICollection<ApplicationUser> IdUsuarios { get; set; }
    }
}
