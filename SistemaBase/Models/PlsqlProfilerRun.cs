using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class PlsqlProfilerRun
    {
        public PlsqlProfilerRun()
        {
            PlsqlProfilerUnits = new HashSet<PlsqlProfilerUnit>();
        }
        public double Runid { get; set; }
        public double? RelatedRun { get; set; }
        public string? RunOwner { get; set; }
        public DateTime? RunDate { get; set; }
        public string? RunComment { get; set; }
        public double? RunTotalTime { get; set; }
        public string? RunSystemInfo { get; set; }
        public string? RunComment1 { get; set; }
        public string? Spare1 { get; set; }
        public virtual ICollection<PlsqlProfilerUnit> PlsqlProfilerUnits { get; set; }
    }
}
