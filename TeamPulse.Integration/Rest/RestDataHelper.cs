using System;
using TeamPulse.Integration.Data;
using System.Collections.Generic;

namespace TeamPulse.Integration.Rest
{
    public class RestDataHelper : IDataHelper
    {
        private readonly string teamPulseRootUrl;
        private readonly string accessCode;

        public RestDataHelper(string teamPulseUrl, string accessCode)
        {
            this.teamPulseRootUrl = teamPulseUrl;
            this.accessCode = accessCode;
        }

        public ProjectTimeEntryType[] GetTimeEntryTypes(int projectId)
        {
            var client = new RestClient(teamPulseRootUrl + string.Format("/api/projects/{0}/projecttimeentrytypes", projectId), accessCode);
            var result = client.Get<ProjectTimeEntryType[]>();

            return result;
        }

        public TimeEntry[] GetTimeEntries()
        {
            var client = new RestClient(teamPulseRootUrl + "/api/timeentries", accessCode);
            var result = client.Get<TimeEntry[]>();

            return result;
        }

        public User GetCurrentUser()
        {
            var client = new RestClient(teamPulseRootUrl + "/api/users/me", accessCode);
            var result = client.Get<User>();

            return result;
        }

        public Project[] GetProjects()
        {
            var client = new RestClient(teamPulseRootUrl + "/api/projects/", accessCode);
            var result = client.Get<Project[]>();

            return result;
        }

        public Task[] GetMyTasks(int projectId)
        {
            var client = new RestClient(teamPulseRootUrl + "/api/tasks/", accessCode);
            var result = client.Get<Task[]>(new Dictionary<string, object> { { "projectId", projectId } });

            return result;
        }

        public TimeEntry LogTime(Task task, float hours, TimeEntryType timeEntryType, int userId)
        {
            var client = new RestClient(teamPulseRootUrl + "/api/timeentries/", accessCode);
            var te = new TimeEntry { date = DateTime.Today, hours = hours, task = task, user = new User { id = userId }, type = timeEntryType.name };
            te = client.Post<TimeEntry>(te);

            return te;
        }

        public TimeEntry[] GetTimeEntries(DateTime date)
        {
            var client = new RestClient(teamPulseRootUrl + "/api/timeentries/", accessCode);
            var result = client.Get<TimeEntry[]>(new Dictionary<string, object>
            {
                { "startDate", date.ToString("yyyy-MM-dd") },
                { "endDate", date.ToString("yyyy-MM-dd") },
            });

            return result;
        }
    }
}