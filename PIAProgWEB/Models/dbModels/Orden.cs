using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PIAProgWEB.Models.dbModels
{
    [Table("Orden")]
    public partial class Orden
    {
        public Orden()
        {
            DetalleOrdens = new HashSet<DetalleOrden>();
        }

        [Key]
        [Column("OrdenID")]
        public int OrdenId { get; set; }
        [Column(TypeName = "date")]
        public DateTime Fecha { get; set; }
        [Column("UsuarioID")]
        public int UsuarioId { get; set; }
        public int EstadoOrden { get; set; }

        [ForeignKey("EstadoOrden")]
        [InverseProperty("Ordens")]
        public virtual EstadoOrden EstadoOrdenNavigation { get; set; } = null!;
        [ForeignKey("UsuarioId")]
        [InverseProperty("Ordens")]
        public virtual ApplicationUser Usuario { get; set; } = null!;
        [InverseProperty("Orden")]
        public virtual ICollection<DetalleOrden> DetalleOrdens { get; set; }
    }
}
