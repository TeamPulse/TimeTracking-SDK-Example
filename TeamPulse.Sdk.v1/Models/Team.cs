
namespace Telerik.TeamPulse.Sdk.Models
{
    public class Team : UserAndDateInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public int projectId { get; set; }
    }
}
