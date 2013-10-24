using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.TeamPulse.Sdk.Common;
using Telerik.TeamPulse.Sdk.Models;

namespace Telerik.TeamPulse.Sdk
{
    public class TimeEntryTypesClient : ApiClientBase
    {
        public TimeEntryTypesClient(string siteUrl, string refreshToken, string username, string password) : 
            base(siteUrl, refreshToken, username, password)
        {
        }

        public TimeEntryTypesClient(string siteUrl, string accessToken) :
            base(siteUrl, accessToken)
        {
        }
        
        /// <summary>
        /// Get all time entry types available for a project
        /// </summary>
        /// <param name="projectId">The ID of the project to which the time entry types are added</param>
        /// <returns>A list of available time entry types</returns>
        public ApiCollection<TimeEntryType> GetByProject(int projectId)
        {
            return this.Get<ApiCollection<TimeEntryType>>(string.Format(ApiUrls.TimeEntryTypes.GetForProject, projectId));
        }

        /// <summary>
        /// Get all time entry types available for a project with filter and sort
        /// </summary>
        /// <param name="projectId">The ID of the project to which the time entry types are added</param>
        /// <param name="oDataOptions">oData formatted query string</param>
        /// <returns>The requested entry types.</returns>
        public ApiCollection<TimeEntryType> GetByProject(string projectId, string oDataOptions)
        {
            return this.Get<ApiCollection<TimeEntryType>>(string.Format("{0}?{1}",
                string.Format(ApiUrls.TimeEntryTypes.GetForProject, projectId), oDataOptions.TrimStart('?')));
        }
    }
}
