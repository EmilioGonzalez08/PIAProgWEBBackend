using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PIAProgWEB.Models
{
    public class ProductosHR
    {

        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = null!;
        public string Descripción { get; set; } = null!;
        public decimal Precio { get; set; }
        public int CategoriaId { get; set; }

        public int IdSubcategoria { get; set; }
        public string Imagen { get; set; } = null!;

    }
}
