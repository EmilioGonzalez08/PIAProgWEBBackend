using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PIAProgWEB.Models.dbModels
{
    [Table("Talla")]
    public partial class Talla
    {
        public Talla()
        {
            ProductoTallas = new HashSet<ProductoTalla>();
        }

        [Key]
        [Column("TallaID")]
        public int TallaId { get; set; }
        [StringLength(50)]
        public string Tamaño { get; set; } = null!;

        [InverseProperty("Talla")]
        public virtual ICollection<ProductoTalla> ProductoTallas { get; set; }
    }
}
