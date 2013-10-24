using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telerik.TeamPulse.Sdk.Models
{
    public class UserAndDateInfo
    {
        public string createdBy { get; set; }
        public DateTime createdAt { get; set; }
        public string lastModifiedBy { get; set; }
        public DateTime lastModifiedAt { get; set; }
    }
}
