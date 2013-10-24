using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Telerik.TeamPulse.Sdk.Common;
using Telerik.TeamPulse.Sdk.Models;
using System.Collections.Generic;

namespace Telerik.TeamPulse.Sdk
{
    public class IterationsClient : ApiClientBase
    {
        public IterationsClient(string siteUrl, string refreshToken, string username, string password) : 
            base(siteUrl, refreshToken, username, password)
        {
        }

        public IterationsClient(string siteUrl, string accessToken) : 
            base(siteUrl, accessToken)
        {
        }

        /// <summary>
        /// Create new iteration
        /// </summary>
        /// <param name="iteration">The iteration to be created</param>
        /// <returns>The created iteration</returns>
        public Iteration Create(Iteration iteration)
        {
            return this.Post<Iteration>(ApiUrls.Iterations.Post, SerializationHelper.SerializeToJson(iteration));
        }

        /// <summary>
        /// Update of an iteration
        /// </summary>
        /// <param name="iterationId">The ID of the iteration</param>
        /// <param name="iterationValues">The fields which has to be updated</param>
        /// <returns>The updated iteration</returns>
        public Iteration Update(int iterationId, Dictionary<string, object> iterationValues)
        {
            return this.Put<Iteration>(string.Format(ApiUrls.Iterations.Put, iterationId), SerializationHelper.SerializeToJson(iterationValues));
        }

        /// <summary>
        /// Get all iterations for specific project
        /// </summary>
        /// <param name="projectId">The ID of the project</param>
        /// <returns>Array of iterations for the specified project</returns>
        public ApiCollection<Iteration> GetByProject(int projectId)
        {
            return this.Get<ApiCollection<Iteration>>(string.Format(ApiUrls.Iterations.GetForProject, projectId));
        }

        /// <summary>
        /// Get all iterations for specific project with filter and sort
        /// </summary>
        /// <param name="projectId">The ID of the project</param>
        /// <param name="oDataOptions">oData formatted query string</param>
        /// <returns>The requested iterations</returns>
        public ApiCollection<Iteration> GetByProject(int projectId, string oDataOptions)
        {
            return this.Get<ApiCollection<Iteration>>(string.Format("{0}?{1}", 
                string.Format(ApiUrls.Iterations.GetForProject, projectId), oDataOptions.TrimStart('?')));
        }

        /// <summary>
        /// Get an iteration by ID
        /// </summary>       
        /// <param name="iterationId">The ID of the iteration</param>
        /// <returns>The selected iteration</returns>
        public Iteration Get(int iterationId)
        {
            return this.Get<Iteration>(string.Format(ApiUrls.Iterations.GetOne, iterationId));
        }

        /// <summary>
        /// Get all iterations
        /// </summary>
        /// <param></param>
        /// <returns>Array of all iterations</returns>
        public ApiCollection<Iteration> GetAll()
        {
            return this.Get<ApiCollection<Iteration>>(ApiUrls.Iterations.GetMany);
        }

        /// <summary>
        /// Get all iterations with filter and sort
        /// </summary>
        /// <param name="oDataOptions">oData formatted query string</param>
        /// <returns>The requested iterations</returns>
        public ApiCollection<Iteration> GetAll(string oDataOptions)
        {
            return this.Get<ApiCollection<Iteration>>(string.Format("{0}?{1}", ApiUrls.Iterations.GetMany, oDataOptions.TrimStart('?')));
        }

        /// <summary>
        /// Delete an iteration
        /// </summary>        
        /// <param name="iterationId">The ID of the iteration to be deleted</param>
        /// <returns></returns>
        public void Delete(int iterationId)
        {
            this.Delete(string.Format(ApiUrls.Iterations.Delete, iterationId));
        }
    }
}
