using MessagePack;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PIAProgWEB.Models
{
    public class ProductoCreateHR
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = null!;
        public string Descripción { get; set; } = null!;
        public decimal Precio { get; set; }
        public int CategoriaId { get; set; }

        public int IdSubcategoria { get; set; }
        public string Imagen { get; set; } = null!;

        [JsonIgnore]
        [IgnoreMember]
        [IgnoreDataMember]
        public SelectList? Categorium { get; set; }
    }
}
