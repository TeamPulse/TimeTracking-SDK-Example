using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Telerik.TeamPulse.Sdk.Common;
using Telerik.TeamPulse.Sdk.Models;
using System.Collections.Generic;

namespace Telerik.TeamPulse.Sdk
{
    public class TeamsClient : ApiClientBase
    {
        public TeamsClient(string siteUrl, string refreshToken, string username, string password) : 
            base(siteUrl, refreshToken, username, password)
        {
        }

        public TeamsClient(string siteUrl, string accessToken) : 
            base(siteUrl, accessToken)
        {
        }

        /// <summary>
        /// Create a new team
        /// </summary>
        /// <param name="team">The team to be created</param>
        /// <returns>The new team</returns>
        public Team Create(Team team)
        {
            return this.Post<Team>(ApiUrls.Teams.Post, SerializationHelper.SerializeToJson(team));
        }

        /// <summary>
        /// Update a team
        /// </summary>
        /// <param name="team">The team with the updated fields</param>
        /// <returns>The updated team</returns>
        public Team Update(Team team)
        {
            return this.Put<Team>(string.Format(ApiUrls.Teams.Put, team.id), SerializationHelper.SerializeToJson(team));
        }

        /// <summary>
        /// Partial update of a team
        /// </summary>
        /// <param name="teamId">The ID of the team</param>
        /// <param name="teamValues">The fields which has to be updated</param>
        /// <returns>The updated team</returns>
        public Team Update(int teamId, Dictionary<string, object> teamValues)
        {
            return this.Put<Team>(string.Format(ApiUrls.Teams.Put, teamId), SerializationHelper.SerializeToJson(teamValues));
        }

        /// <summary>
        /// Get all teams for a project
        /// </summary>
        /// <param name="projectId">The ID of the project to which the teams belong to</param>
        /// <returns>The project teams and metadata for them</returns>
        public ApiCollection<Team> GetByProject(int projectId)
        {
            return this.Get<ApiCollection<Team>>(string.Format(ApiUrls.Teams.GetMany, projectId));
        }

        /// <summary>
        /// Get all teams with filter and sort
        /// </summary>
        /// <param name="projectId">The ID of the project to which the teams belong to</param>
        /// <param name="oDataOptions">oData formatted query string</param>
        /// <returns>The requested entry types.</returns>
        public ApiCollection<Team> GetByProject(string projectId, string oDataOptions)
        {
            return this.Get<ApiCollection<Team>>(string.Format("{0}?{1}", 
                string.Format(ApiUrls.Teams.GetMany, projectId), oDataOptions.TrimStart('?')));
        }

        /// <summary>
        /// Get a team
        /// </summary>       
        /// <param name="teamId">The ID of the team</param>
        /// <returns></returns>
        public Team Get(int teamId)
        {
            return this.Get<Team>(string.Format(ApiUrls.Teams.GetOne, teamId));
        }

        /// <summary>
        /// Delete a team
        /// </summary>
        /// <param name="teamId">The ID of the team to be deleted</param>
        public void Delete(int teamId)
        {
            this.Delete(string.Format(ApiUrls.Teams.Delete, teamId));
        }
    }
}
