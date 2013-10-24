using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.TeamPulse.Sdk.Common;
using Telerik.TeamPulse.Sdk.Models;

namespace Telerik.TeamPulse.Sdk
{
    public class TimeEntriesClient : ApiClientBase
    {
        public TimeEntriesClient(string siteUrl, string refreshToken, string username, string password) : 
            base(siteUrl, refreshToken, username, password)
        {
        }

        public TimeEntriesClient(string siteUrl, string accessToken) :
            base(siteUrl, accessToken)
        {
        }

        /// <summary>
        /// Create a new time entry
        /// </summary>
        /// <param name="timeEntry">The time entry to be created</param>
        /// <returns>The new time entry</returns>
        public TimeEntry Create(TimeEntry timeEntry)
        {
            return this.Post<TimeEntry>(ApiUrls.TimeEntries.Post, SerializationHelper.SerializeToJson(timeEntry));
        }

        /// <summary>
        /// Update a time entry
        /// </summary>
        /// <param name="timeEntry">The time entry with the updated fields</param>
        /// <returns>The updated time entry</returns>
        public TimeEntry Update(TimeEntry timeEntry)
        {
            return this.Put<TimeEntry>(string.Format(ApiUrls.TimeEntries.Put, timeEntry.id), SerializationHelper.SerializeToJson(timeEntry));
        }

        /// <summary>
        /// Partial update of a time entry
        /// </summary>
        /// <param name="timeEntryId">The ID of the time entry</param>
        /// <param name="timeEntryValues">The fields which has to be updated</param>
        /// <returns>The updated time entry</returns>
        public TimeEntry Update(int timeEntryId, Dictionary<string, object> timeEntryValues)
        {            
            return this.Put<TimeEntry>(string.Format(ApiUrls.TimeEntries.Put, timeEntryId), SerializationHelper.SerializeToJson(timeEntryValues));
        }

        /// <summary>
        /// Get all time entries
        /// </summary>        
        /// <returns>The time entries and metadata for them. Max 100 are returned.</returns>
        public ApiCollection<TimeEntry> Get()
        {            
            return this.Get<ApiCollection<TimeEntry>>(string.Format(ApiUrls.TimeEntries.GetMany));
        }

        /// <summary>
        /// Get all time entries with filter and sort
        /// </summary>
        /// <param name="oDataOptions">oData formatted query string</param>
        /// <returns>The requested time entries.</returns>
        public ApiCollection<TimeEntry> Get(string oDataOptions)
        {
            return this.Get<ApiCollection<TimeEntry>>(string.Format("{0}?{1}", ApiUrls.TimeEntries.GetMany, oDataOptions.TrimStart('?')));
        }

        /// <summary>
        /// Get all time entries for a task
        /// </summary>
        /// <param name="taskId">The ID of the task to which the time entries are added</param>
        /// <returns>The task time entries and metadata for them</returns>
        public ApiCollection<TimeEntry> GetByTask(int taskId)
        {
            return this.Get<ApiCollection<TimeEntry>>(string.Format(ApiUrls.TimeEntries.GetForTask, taskId));
        }

        /// <summary>
        /// Get all time entries for a task with filter and sort
        /// </summary>
        /// <param name="taskId">The ID of the task to which the time entries are added</param>
        /// <param name="oDataOptions">oData formatted query string</param>
        /// <returns>The requested time entries.</returns>
        public ApiCollection<TimeEntry> GetByTask(int taskId, string oDataOptions)
        {
            return this.Get<ApiCollection<TimeEntry>>(string.Format("{0}?{1}", 
                string.Format(ApiUrls.TimeEntries.GetForTask, taskId), oDataOptions.TrimStart('?')));
        }

        /// <summary>
        /// Get a time entry
        /// </summary>       
        /// <param name="timeEntryId">The ID of the time entry</param>
        /// <returns></returns>
        public TimeEntry Get(int timeEntryId)
        {
            return this.Get<TimeEntry>(string.Format(ApiUrls.TimeEntries.GetOne, timeEntryId));
        }

        /// <summary>
        /// Delete a time entry
        /// </summary>
        /// <param name="timeEntryId">The ID of the time entry to be deleted</param>
        public void Delete(int timeEntryId)
        {
            this.Delete(string.Format(ApiUrls.TimeEntries.Delete, timeEntryId));
        }
    }
}
