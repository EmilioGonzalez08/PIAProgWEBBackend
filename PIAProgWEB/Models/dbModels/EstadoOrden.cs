using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PIAProgWEB.Models.dbModels
{
    [Table("Estado_orden")]
    public partial class EstadoOrden
    {
        public EstadoOrden()
        {
            Ordens = new HashSet<Orden>();
        }

        [Key]
        [Column("EstadoID")]
        public int EstadoId { get; set; }
        [StringLength(50)]
        public string Estado { get; set; } = null!;

        [InverseProperty("EstadoOrdenNavigation")]
        public virtual ICollection<Orden> Ordens { get; set; }
    }
}
