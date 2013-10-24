
using System;
using System.Collections.Generic;
namespace Telerik.TeamPulse.Sdk.Models
{
    public class Comment : UserAndDateInfo
    {
        public int id { get; set; }
        public int workItemId { get; set; }
        public int projectId { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        //public IList<Attachment> attachments { get; set; }
    }
}
