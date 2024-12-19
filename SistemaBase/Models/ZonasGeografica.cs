using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class ZonasGeografica
    {
        public ZonasGeografica()
        {
            ZonasUbicacions = new HashSet<ZonasUbicacion>();
        }
        public string CodZona { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? CodGrupo { get; set; }
        public Guid Rowid { get; set; }
        public virtual ICollection<ZonasUbicacion> ZonasUbicacions { get; set; }
    }
}
