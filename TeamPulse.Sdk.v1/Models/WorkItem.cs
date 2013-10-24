using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Telerik.TeamPulse.Sdk.Models
{
    public class WorkItem : UserAndDateInfo
    {
        public int id { get; set; }
        public string type { get; set; }
        public int projectId { get; set; }
        /// <summary>
        /// Work item fields
        /// </summary>
        public Dictionary<string, object> fields { get; set; }
    }
}
