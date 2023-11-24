using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIAProgWEB.Models.dbModels
{
    public partial class Subcategorium
    {
        public Subcategorium()
        {
            Productos = new HashSet<Producto>();
        }

        [Key]
        public int IdSubcategoria { get; set; }
        [Column("CategoriaID")]
        public int CategoriaId { get; set; }
        [StringLength(50)]
        public string NombreSubcategoria { get; set; } = null!;

        [ForeignKey("CategoriaId")]
        [InverseProperty("Subcategoria")]
        public virtual Categorium Categoria { get; set; } = null!;

        [InverseProperty("SubCategoria")]
        public virtual ICollection<Producto> Productos { get; set; }
    }
}
