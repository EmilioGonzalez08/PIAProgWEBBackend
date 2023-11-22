using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PIAProgWEB.Models.dbModels
{
    public partial class Subcategorium
    {
        [Key]
        public int IdSubcategoria { get; set; }
        [Column("CategoriaID")]
        public int CategoriaId { get; set; }
        [StringLength(50)]
        public string NombreSubcategoria { get; set; } = null!;

        [ForeignKey("CategoriaId")]
        [InverseProperty("Subcategoria")]
        public virtual Categorium Categoria { get; set; } = null!;
    }
}
