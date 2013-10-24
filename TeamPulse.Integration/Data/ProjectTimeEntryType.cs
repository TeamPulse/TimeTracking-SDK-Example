namespace TeamPulse.Integration.Data
{
    public class ProjectTimeEntryType
    {
        public int id { get; set; }

        public TeamPulse.Integration.Data.TimeEntryType timeEntryType { get; set; }

        public Project project { get; set; }

        public int? sequenceNumber { get; set; }

        public long iversion { get; set; }
    }
}