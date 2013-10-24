
using System;
namespace Telerik.TeamPulse.Sdk.Models
{
    public class Iteration : UserAndDateInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public int projectId { get; set; }
        public int parentId { get; set; }
        public int sequence { get; set; }
        public float? minCapacity { get; set; }
        public float? maxCapacity { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}
