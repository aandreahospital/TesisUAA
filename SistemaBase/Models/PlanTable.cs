using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class PlanTable
    {
        public string? StatementId { get; set; }
        public DateTime? Timestamp { get; set; }
        public string? Remarks { get; set; }
        public string? Operation { get; set; }
        public string? Options { get; set; }
        public string? ObjectNode { get; set; }
        public string? ObjectOwner { get; set; }
        public string? ObjectName { get; set; }
        public double? ObjectInstance { get; set; }
        public string? ObjectType { get; set; }
        public double? SearchColumns { get; set; }
        public double? Id { get; set; }
        public double? ParentId { get; set; }
        public double? Position { get; set; }
        public string? Other { get; set; }
        public string? Optimizer { get; set; }
        public string? OtherTag { get; set; }
        public string? PartitionStart { get; set; }
        public string? PartitionStop { get; set; }
        public string? Distribution { get; set; }
    }
}
