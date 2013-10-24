
using System;
namespace Telerik.TeamPulse.Sdk.Models
{
    public class Area : UserAndDateInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public int projectId { get; set; }
        public int parentId { get; set; }
        public int sequence { get; set; }
    }
}
