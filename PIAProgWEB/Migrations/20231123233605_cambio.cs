using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PIAProgWEB.Migrations
{
    public partial class cambio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producto_Categoria_CategoriaID",
                table: "Producto");

            migrationBuilder.AddForeignKey(
                name: "FK_Producto_Subcategoria_CategoriaID",
                table: "Producto",
                column: "CategoriaID",
                principalTable: "Subcategoria",
                principalColumn: "IdSubcategoria",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producto_Subcategoria_CategoriaID",
                table: "Producto");

            migrationBuilder.AddForeignKey(
                name: "FK_Producto_Categoria_CategoriaID",
                table: "Producto",
                column: "CategoriaID",
                principalTable: "Categoria",
                principalColumn: "CategoriaID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
