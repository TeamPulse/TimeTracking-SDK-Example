using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Telerik.TeamPulse.Sdk.Common;
using Telerik.TeamPulse.Sdk.Models;
using System.Collections.Generic;

namespace Telerik.TeamPulse.Sdk
{
    public class AreasClient : ApiClientBase
    {
        public AreasClient(string siteUrl, string refreshToken, string username, string password) : 
            base(siteUrl, refreshToken, username, password)
        {
        }

        public AreasClient(string siteUrl, string accessToken) :
            base(siteUrl, accessToken)
        {
        }

        /// <summary>
        /// Create new area
        /// </summary>
        /// <param name="area">The area to be created</param>
        /// <returns>The created area</returns>
        public Area Create(Area area)
        {
            return this.Post<Area>(ApiUrls.Areas.Post, SerializationHelper.SerializeToJson(area));
        }

        /// <summary>
        /// Update of an area
        /// </summary>
        /// <param name="areaId">The ID of the area</param>
        /// <param name="areaValues">The fields which has to be updated</param>
        /// <returns>The updated area</returns>
        public Area Update(int areaId, Dictionary<string, object> areaValues)
        {
            return this.Put<Area>(string.Format(ApiUrls.Areas.Put, areaId), SerializationHelper.SerializeToJson(areaValues));
        }

        /// <summary>
        /// Get all areas for specific project
        /// </summary>
        /// <param name="projectId">The ID of the project</param>
        /// <returns>Array of areas for the specified project</returns>
        public ApiCollection<Area> GetByProject(int projectId)
        {
            return this.Get<ApiCollection<Area>>(string.Format(ApiUrls.Areas.GetForProject, projectId));
        }

        /// <summary>
        /// Get all areas for specific project with filter and sort
        /// </summary>
        /// <param name="projectId">The ID of the project</param>
        /// <param name="oDataOptions">oData formatted query string</param>
        /// <returns>The requested areas</returns>
        public ApiCollection<Area> GetByProject(int projectId, string oDataOptions)
        {
            return this.Get<ApiCollection<Area>>(string.Format("{0}?{1}", 
                string.Format(ApiUrls.Areas.GetForProject, projectId), oDataOptions.TrimStart('?')));
        }

        /// <summary>
        /// Get an area by ID
        /// </summary>       
        /// <param name="areaId">The ID of the area</param>
        /// <returns></returns>
        public Area Get(int areaId)
        {
            return this.Get<Area>(string.Format(ApiUrls.Areas.GetOne, areaId));
        }

        /// <summary>
        /// Get all areas
        /// </summary>
        /// <param></param>
        /// <returns>Array of all areas</returns>
        public ApiCollection<Area> GetAll()
        {
            return this.Get<ApiCollection<Area>>(ApiUrls.Areas.GetMany);
        }

        /// <summary>
        /// Get all areas with filter and sort
        /// </summary>
        /// <param name="oDataOptions">oData formatted query string</param>
        /// <returns>The requested areas</returns>
        public ApiCollection<Area> GetAll(string oDataOptions)
        {
            return this.Get<ApiCollection<Area>>(string.Format("{0}?{1}", ApiUrls.Areas.GetMany, oDataOptions.TrimStart('?')));
        }

        /// <summary>
        /// Delete an area
        /// </summary>        
        /// <param name="areaId">The ID of the area to be deleted</param>
        /// <returns></returns>
        public void Delete(int areaId)
        {
            this.Delete(string.Format(ApiUrls.Areas.Delete, areaId));
        }
    }
}
