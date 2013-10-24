using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telerik.TeamPulse.Sdk.Models
{
    public class TimeEntry : UserAndDateInfo
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public float hours { get; set; }
        public string type { get; set; }
        public int userId { get; set; }
        public int taskId { get; set; }
        public string note { get; set; }
        public int week { get; set; }
    }
}
