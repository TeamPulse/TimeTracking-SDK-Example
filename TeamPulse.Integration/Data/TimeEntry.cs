using System;

namespace TeamPulse.Integration.Data
{
    public class TimeEntry
    {
        public int id { get; set; }

        public DateTime date { get; set; }

        public float? hours { get; set; }

        public string type { get; set; }

        public User user { get; set; }

        public TeamPulse.Integration.Data.Task task { get; set; }

        public string note { get; set; }

        public long iversion { get; set; }
    }
}