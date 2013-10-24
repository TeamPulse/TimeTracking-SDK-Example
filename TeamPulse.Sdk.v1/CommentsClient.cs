using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Telerik.TeamPulse.Sdk.Common;
using Telerik.TeamPulse.Sdk.Models;
using System.Collections.Generic;

namespace Telerik.TeamPulse.Sdk
{
    public class CommentsClient : ApiClientBase
    {
        public CommentsClient(string siteUrl, string refreshToken, string username, string password) : 
            base(siteUrl, refreshToken, username, password)
        {
        }

        public CommentsClient(string siteUrl, string accessToken) : 
            base(siteUrl, accessToken)
        {
        }

        /// <summary>
        /// Create new comment
        /// </summary>
        /// <param name="comment">The comment to be created</param>
        /// <returns>The created comment</returns>
        public Comment Create(Comment comment)
        {
            return this.Post<Comment>(ApiUrls.Comments.Post, SerializationHelper.SerializeToJson(comment));
        }

        /// <summary>
        /// Get all comments for specific workitem
        /// </summary>
        /// <param name="workItemId">The ID of the workitem</param>
        /// <returns>Array of comments for the specified workitem</returns>
        public ApiCollection<Comment> GetByWorkItem(int workItemId)
        {
            return this.Get<ApiCollection<Comment>>(string.Format(ApiUrls.Comments.GetForWorkItem, workItemId));
        }

        /// <summary>
        /// Get all comments for specific workitem with filter and sort
        /// </summary>
        /// <param name="workItemId">The ID of the workitem</param>
        /// <param name="oDataOptions">oData formatted query string</param>
        /// <returns>The requested entry types.</returns>
        public ApiCollection<Comment> GetByWorkItem(int workItemId, string oDataOptions)
        {
            return this.Get<ApiCollection<Comment>>(string.Format("{0}?{1}", 
                string.Format(ApiUrls.Comments.GetForWorkItem, workItemId), oDataOptions.TrimStart('?')));
        }

        /// <summary>
        /// Get a comment by ID
        /// </summary>       
        /// <param name="commentId">The ID of the comment</param>
        /// <returns></returns>
        public Comment Get(int commentId)
        {
            return this.Get<Comment>(string.Format(ApiUrls.Comments.GetOne, commentId));
        }

        /// <summary>
        /// Get all comments
        /// </summary>
        /// <param></param>
        /// <returns>Array of all comments</returns>
        public ApiCollection<Comment> GetAll()
        {
            return this.Get<ApiCollection<Comment>>(ApiUrls.Comments.GetMany);
        }

        /// <summary>
        /// Get all comments with filter and sort
        /// </summary>
        /// <param name="oDataOptions">oData formatted query string</param>
        /// <returns>The requested entry types.</returns>
        public ApiCollection<Comment> GetAll(string oDataOptions)
        {
            return this.Get<ApiCollection<Comment>>(string.Format("{0}?{1}", ApiUrls.Comments.GetMany, oDataOptions.TrimStart('?')));
        }

        /// <summary>
        /// Delete a comment
        /// </summary>        
        /// <param name="commentId">The ID of the comment to be deleted</param>
        /// <returns></returns>
        public void Delete(int commentId)
        {
            this.Delete(string.Format(ApiUrls.Comments.Delete, commentId));
        }
    }
}
