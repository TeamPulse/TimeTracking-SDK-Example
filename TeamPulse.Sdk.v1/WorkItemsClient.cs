using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.TeamPulse.Sdk.Common;
using Telerik.TeamPulse.Sdk.Models;

namespace Telerik.TeamPulse.Sdk
{
    public class WorkItemsClient : ApiClientBase
    {
        public WorkItemsClient(string siteUrl, string refreshToken, string username, string password) : 
            base(siteUrl, refreshToken, username, password)
        {
        }

        public WorkItemsClient(string siteUrl, string accessToken) :
            base(siteUrl, accessToken)
        {
        }

        /// <summary>
        /// Create a new work item
        /// </summary>
        /// <param name="workItem">The work item to be created</param>
        /// <returns>The created work item</returns>
        public WorkItem Create(WorkItem workItem)
        {
            return this.Post<WorkItem>(ApiUrls.WorkItems.Post, SerializationHelper.SerializeToJson(workItem));
        }

        /// <summary>
        /// Update a work item
        /// </summary>
        /// <param name="workItem">The work item with the updated fields</param>
        /// <returns>The updated work item</returns>
        public WorkItem Update(WorkItem workItem)
        {
            return this.Put<WorkItem>(string.Format(ApiUrls.WorkItems.Put, workItem.id), SerializationHelper.SerializeToJson(workItem.fields));
        }

        /// <summary>
        /// Partial update of a work item
        /// </summary>
        /// <param name="workItemId">The ID of the work item</param>
        /// <param name="workItemValues">The fields which has to be updated</param>
        /// <returns>The updated work item</returns>
        public WorkItem Update(int workItemId, Dictionary<string, object> workItemValues)
        {
            return this.Put<WorkItem>(string.Format(ApiUrls.WorkItems.Put, workItemId), SerializationHelper.SerializeToJson(workItemValues));
        }

        /// <summary>
        /// Get work items. Returns top 100 by default. OData options can be used to filter and page the items
        /// </summary>
        /// <returns>The requested work items and metadata for them</returns>
        public ApiCollection<WorkItem> Get()
        {
            return this.Get<ApiCollection<WorkItem>>(ApiUrls.WorkItems.GetMany);
        }

        /// <summary>
        /// Get work items with filter and sort
        /// </summary>
        /// <param name="oDataOptions">oData formatted query string</param>
        /// <returns>The requested work items</returns>
        public ApiCollection<WorkItem> Get(string oDataOptions)
        {
            return this.Get<ApiCollection<WorkItem>>(string.Format("{0}?{1}", ApiUrls.WorkItems.GetMany, oDataOptions.TrimStart('?')));
        }

        /// <summary>
        /// Get a work item
        /// </summary>       
        /// <param name="workItemId">The ID of the work item</param>
        /// <returns>The requested work item</returns>
        public WorkItem Get(int workItemId)
        {
            return this.Get<WorkItem>(string.Format(ApiUrls.WorkItems.GetOne, workItemId));
        }

        /// <summary>
        /// Delete a work item
        /// </summary>
        /// <param name="workItemId">The ID of the work item to be deleted</param>
        public void Delete(int workItemId)
        {
            this.Delete(string.Format(ApiUrls.WorkItems.Delete, workItemId));
        }
    }
}
