using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Telerik.TeamPulse.Sdk.Common;
using Telerik.TeamPulse.Sdk.Models;
using System.Collections.Generic;

namespace Telerik.TeamPulse.Sdk
{
    public class ProjectsClient : ApiClientBase
    {
        public ProjectsClient(string siteUrl, string refreshToken, string username, string password) : 
            base(siteUrl, refreshToken, username, password)
        {
        }

        public ProjectsClient(string siteUrl, string accessToken) :
            base(siteUrl, accessToken)
        {
        }

        /// <summary>
        /// Get a project by ID
        /// </summary>       
        /// <param name="id">The ID of the project</param>
        /// <returns></returns>
        public Project Get(int id)
        {
            return this.Get<Project>(string.Format(ApiUrls.Projects.GetOne, id));
        }

        /// <summary>
        /// Get all projects
        /// </summary>
        /// <param></param>
        /// <returns>Array of all projects</returns>
        public ApiCollection<Project> GetAll()
        {
            return this.Get<ApiCollection<Project>>(ApiUrls.Projects.GetMany);
        }

        /// <summary>
        /// Get all projects with filter and sort
        /// </summary>
        /// <param name="oDataOptions">oData formatted query string</param>
        /// <returns>The requested entry types.</returns>
        public ApiCollection<Project> GetAll(string oDataOptions)
        {
            return this.Get<ApiCollection<Project>>(string.Format("{0}?{1}", ApiUrls.Projects.GetMany, oDataOptions.TrimStart('?')));
        }
    }
}
