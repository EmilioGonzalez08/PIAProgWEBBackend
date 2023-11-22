using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PIAProgWEB.Models.dbModels
{
    public partial class Estacione
    {
        public Estacione()
        {
            Novedads = new HashSet<Novedad>();
        }

        [Key]
        public int IdEstacion { get; set; }
        [StringLength(50)]
        public string Estacion { get; set; } = null!;

        [InverseProperty("IdEstacionNavigation")]
        public virtual ICollection<Novedad> Novedads { get; set; }
    }
}
