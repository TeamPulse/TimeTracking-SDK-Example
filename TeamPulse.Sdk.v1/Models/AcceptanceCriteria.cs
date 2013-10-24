
using System;
namespace Telerik.TeamPulse.Sdk.Models
{
    public class AcceptanceCriteria : UserAndDateInfo
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int workItemId { get; set; }
        public int projectId { get; set; }
        public string status { get; set; }
    }
}
