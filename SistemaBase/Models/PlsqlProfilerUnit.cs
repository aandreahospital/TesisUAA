using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class PlsqlProfilerUnit
    {
        public PlsqlProfilerUnit()
        {
            PlsqlProfilerData = new HashSet<PlsqlProfilerDatum>();
        }
        public double Runid { get; set; }
        public double UnitNumber { get; set; }
        public string? UnitType { get; set; }
        public string? UnitOwner { get; set; }
        public string? UnitName { get; set; }
        public DateTime? UnitTimestamp { get; set; }
        public double TotalTime { get; set; }
        public double? Spare1 { get; set; }
        public double? Spare2 { get; set; }
        public virtual PlsqlProfilerRun Run { get; set; } = null!;
        public virtual ICollection<PlsqlProfilerDatum> PlsqlProfilerData { get; set; }
    }
}
