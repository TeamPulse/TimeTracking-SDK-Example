namespace TeamPulse.Integration.Data
{
    public class Task
    {
        public int id { get; set; }

        public string name { get; set; }

        public Project project { get; set; }

        public TeamPulse.Integration.Data.WorkItem workItem { get; set; }
    }
}