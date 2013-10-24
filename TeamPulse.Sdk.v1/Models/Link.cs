
using System;
namespace Telerik.TeamPulse.Sdk.Models
{
    public class Link : UserAndDateInfo
    {
        public int id { get; set; }
        public int projectId { get; set; }
        public int workItemId { get; set; }
        public string displayText { get; set; }
        public string url { get; set; }
    }
}
