using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telerik.TeamPulse.Sdk.Models
{
    public class Attachment : UserAndDateInfo
    {
        public int id { get; set; }        
        public int projectId { get; set; }
        public int workItemId { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public int? size { get; set; }
        public string url { get; set; }        
        public string downloadUrl { get; set; }
        public int? commentId { get; set; }
        public string friendlySize { get; set; }
    }
}
