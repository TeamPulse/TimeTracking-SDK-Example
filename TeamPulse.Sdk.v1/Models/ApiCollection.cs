using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telerik.TeamPulse.Sdk.Models
{
    public class ApiCollection<T>
    {
        public int totalResults;
        public int? top;
        public int? skip;
        public T[] results;
    }
}
