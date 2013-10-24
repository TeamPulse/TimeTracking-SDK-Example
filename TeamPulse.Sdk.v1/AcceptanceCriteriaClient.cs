using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Telerik.TeamPulse.Sdk.Common;
using Telerik.TeamPulse.Sdk.Models;
using System.Collections.Generic;

namespace Telerik.TeamPulse.Sdk
{
    public class AcceptanceCriteriaClient : ApiClientBase
    {
        public AcceptanceCriteriaClient(string siteUrl, string refreshToken, string username, string password) : 
            base(siteUrl, refreshToken, username, password)
        {
        }

        public AcceptanceCriteriaClient(string siteUrl, string accessToken) :
            base(siteUrl, accessToken)
        {
        }

        /// <summary>
        /// Create new accpetance criteria
        /// </summary>
        /// <param name="accpetanceCriteria">The accpetance criteria to be created</param>
        /// <returns>The created accpetance criteria</returns>
        public AcceptanceCriteria Create(AcceptanceCriteria accpetanceCriteria)
        {
            return this.Post<AcceptanceCriteria>(ApiUrls.AcceptanceCriterias.Post, SerializationHelper.SerializeToJson(accpetanceCriteria));
        }

        /// <summary>
        /// Update of a accpetance criteria
        /// </summary>
        /// <param name="accpetanceCriteriaId">The ID of the accpetance criteria</param>
        /// <param name="accpetanceCriteriaValues">The fields which has to be updated</param>
        /// <returns>The updated accpetance criteria</returns>
        public AcceptanceCriteria Update(int accpetanceCriteriaId, Dictionary<string, object> accpetanceCriteriaValues)
        {
            return this.Put<AcceptanceCriteria>(string.Format(ApiUrls.AcceptanceCriterias.Put, accpetanceCriteriaId), SerializationHelper.SerializeToJson(accpetanceCriteriaValues));
        }

        /// <summary>
        /// Get all accpetance criterias for specific workitem
        /// </summary>
        /// <param name="workItemId">The ID of the workitem</param>
        /// <returns>Array of accpetance criterias for the specified workitem</returns>
        public ApiCollection<AcceptanceCriteria> GetByWorkItem(int workItemId)
        {
            return this.Get<ApiCollection<AcceptanceCriteria>>(string.Format(ApiUrls.AcceptanceCriterias.GetForWorkItem, workItemId));
        }

        /// <summary>
        /// Get all accpetance criterias for specific workitem with filter and sort
        /// </summary>
        /// <param name="workItemId">The ID of the workitem</param>
        /// <param name="oDataOptions">oData formatted query string</param>
        /// <returns>The requested accpetance criterias</returns>
        public ApiCollection<AcceptanceCriteria> GetByWorkItem(int workItemId, string oDataOptions)
        {
            return this.Get<ApiCollection<AcceptanceCriteria>>(string.Format("{0}?{1}", 
                string.Format(ApiUrls.AcceptanceCriterias.GetForWorkItem, workItemId), oDataOptions.TrimStart('?')));
        }

        /// <summary>
        /// Get a accpetance criteria by ID
        /// </summary>       
        /// <param name="accpetanceCriteriaId">The ID of the accpetance criteria</param>
        /// <returns></returns>
        public AcceptanceCriteria Get(int accpetanceCriteriaId)
        {
            return this.Get<AcceptanceCriteria>(string.Format(ApiUrls.AcceptanceCriterias.GetOne, accpetanceCriteriaId));
        }

        /// <summary>
        /// Get all accpetance criterias
        /// </summary>
        /// <param></param>
        /// <returns>Array of all accpetance criterias</returns>
        public ApiCollection<AcceptanceCriteria> GetAll()
        {
            return this.Get<ApiCollection<AcceptanceCriteria>>(ApiUrls.AcceptanceCriterias.GetMany);
        }

        /// <summary>
        /// Get all accpetance criterias with filter and sort
        /// </summary>
        /// <param name="oDataOptions">oData formatted query string</param>
        /// <returns>The requested accpetance criterias</returns>
        public ApiCollection<AcceptanceCriteria> GetAll(string oDataOptions)
        {
            return this.Get<ApiCollection<AcceptanceCriteria>>(string.Format("{0}?{1}", ApiUrls.AcceptanceCriterias.GetMany, oDataOptions.TrimStart('?')));
        }

        /// <summary>
        /// Delete a accpetance criteria
        /// </summary>        
        /// <param name="accpetanceCriteriaId">The ID of the accpetance criteria to be deleted</param>
        /// <returns></returns>
        public void Delete(int accpetanceCriteriaId)
        {
            this.Delete(string.Format(ApiUrls.AcceptanceCriterias.Delete, accpetanceCriteriaId));
        }
    }
}
