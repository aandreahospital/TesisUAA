using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class PlsqlProfilerDatum
    {
        public double Runid { get; set; }
        public double UnitNumber { get; set; }
        public double Line { get; set; }
        public double? TotalOccur { get; set; }
        public double? TotalTime { get; set; }
        public double? MinTime { get; set; }
        public double? MaxTime { get; set; }
        public double? Spare1 { get; set; }
        public double? Spare2 { get; set; }
        public double? Spare3 { get; set; }
        public double? Spare4 { get; set; }
        public virtual PlsqlProfilerUnit PlsqlProfilerUnit { get; set; } = null!;
    }
}
