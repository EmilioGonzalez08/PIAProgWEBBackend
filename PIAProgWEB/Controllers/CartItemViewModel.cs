using PIAProgWEB.Models.dbModels;

namespace PIAProgWEB.Controllers
{
    internal class CartItemViewModel
    {
        public int UsuarioId { get; set; }
        public int ProductioId { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public string Size { get; set; }
        public Producto Productio { get; set; }
        public ApplicationUser Usuario { get; set; }
    }
}