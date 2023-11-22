using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PIAProgWEB.Models
{
    public class SubcategoriasHR
    {

        public int IdSubcategoria { get; set; }
        public int CategoriaId { get; set; }
        public string NombreSubcategoria { get; set; } = null!;

    }
}
