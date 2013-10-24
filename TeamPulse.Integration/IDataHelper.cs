using System;
using System.Linq;
using TeamPulse.Integration.Data;

namespace TeamPulse.Integration
{
    public interface IDataHelper
    {
        User GetCurrentUser();

        Project[] GetProjects();

        Task[] GetMyTasks(int projectId);

        TimeEntry LogTime(Task task, float hours, TimeEntryType timeEntryType, int userId);

        ProjectTimeEntryType[] GetTimeEntryTypes(int projectId);

        TimeEntry[] GetTimeEntries(DateTime date);
    }
}
