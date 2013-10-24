using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telerik.TeamPulse.Sdk.Models
{
    public class Project : UserAndDateInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string template { get; set; }
        public string description { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int defaultIterationLength { get; set; }
    }
}
