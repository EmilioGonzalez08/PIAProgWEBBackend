using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIAProgWEB.Models.dbModels
{
    public partial class Categorium
    {
        public Categorium()
        {
            Subcategoria = new HashSet<Subcategorium>();
        }

        [Key]
        [Column("CategoriaID")]
        public int CategoriaId { get; set; }
        [StringLength(50)]
        public string Categoria { get; set; } = null!;

        
        [InverseProperty("Categoria")]
        public virtual ICollection<Subcategorium> Subcategoria { get; set; }
    }
}
