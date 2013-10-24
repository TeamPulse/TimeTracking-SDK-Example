using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telerik.TeamPulse.Sdk.Models
{
    public class Activity : UserAndDateInfo
    {
        public int id { get; set; }
        public int? projectId { get; set; }
        public string content { get; set; }
        public string iconUrl { get; set; }
        public string author { get; set; }
        public bool isFlagged { get; set; }
        public bool isRead { get; set; }
        public string system { get; set; }
        public string systemEvent { get; set; }
        public int? workItemId { get; set; }
        public string commentType { get; set; }
    }
}
