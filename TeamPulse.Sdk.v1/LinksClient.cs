using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Telerik.TeamPulse.Sdk.Common;
using Telerik.TeamPulse.Sdk.Models;
using System.Collections.Generic;

namespace Telerik.TeamPulse.Sdk
{
    public class LinksClient : ApiClientBase
    {
        public LinksClient(string siteUrl, string refreshToken, string username, string password) : 
            base(siteUrl, refreshToken, username, password)
        {
        }

        public LinksClient(string siteUrl, string accessToken) :
            base(siteUrl, accessToken)
        {
        }

        /// <summary>
        /// Create new link
        /// </summary>
        /// <param name="link">The link to be created</param>
        /// <returns>The created link</returns>
        public Link Create(Link link)
        {
            return this.Post<Link>(ApiUrls.Links.Post, SerializationHelper.SerializeToJson(link));
        }

        /// <summary>
        /// Update of a link
        /// </summary>
        /// <param name="linkId">The ID of the link</param>
        /// <param name="linkValues">The fields which has to be updated</param>
        /// <returns>The updated link</returns>
        public Link Update(int linkId, Dictionary<string, object> linkValues)
        {
            return this.Put<Link>(string.Format(ApiUrls.Links.Put, linkId), SerializationHelper.SerializeToJson(linkValues));
        }

        /// <summary>
        /// Get all links for specific workitem
        /// </summary>
        /// <param name="workItemId">The ID of the workitem</param>
        /// <returns>Array of links for the specified workitem</returns>
        public ApiCollection<Link> GetByWorkItem(int workItemId)
        {
            return this.Get<ApiCollection<Link>>(string.Format(ApiUrls.Links.GetForWorkItem, workItemId));
        }

        /// <summary>
        /// Get all links for specific workitem with filter and sort
        /// </summary>
        /// <param name="workItemId">The ID of the workitem</param>
        /// <param name="oDataOptions">oData formatted query string</param>
        /// <returns>The requested links.</returns>
        public ApiCollection<Link> GetByWorkItem(int workItemId, string oDataOptions)
        {
            return this.Get<ApiCollection<Link>>(string.Format("{0}?{1}", 
                string.Format(ApiUrls.Links.GetForWorkItem, workItemId), oDataOptions.TrimStart('?')));
        }

        /// <summary>
        /// Get a link by ID
        /// </summary>       
        /// <param name="linkId">The ID of the link</param>
        /// <returns></returns>
        public Link Get(int linkId)
        {
            return this.Get<Link>(string.Format(ApiUrls.Links.GetOne, linkId));
        }

        /// <summary>
        /// Get all links
        /// </summary>
        /// <param></param>
        /// <returns>Array of all links</returns>
        public ApiCollection<Link> GetAll()
        {
            return this.Get<ApiCollection<Link>>(ApiUrls.Links.GetMany);
        }

        /// <summary>
        /// Get all links with filter and sort
        /// </summary>
        /// <param name="workItemId">The ID of the workitem</param>
        /// <param name="oDataOptions">oData formatted query string</param>
        /// <returns>The requested links.</returns>
        public ApiCollection<Link> GetAll(string oDataOptions)
        {
            return this.Get<ApiCollection<Link>>(string.Format("{0}?{1}", ApiUrls.Links.GetMany, oDataOptions.TrimStart('?')));
        }

        /// <summary>
        /// Delete a link
        /// </summary>        
        /// <param name="linkId">The ID of the link to be deleted</param>
        /// <returns></returns>
        public void Delete(int linkId)
        {
            this.Delete(string.Format(ApiUrls.Links.Delete, linkId));
        }
    }
}
