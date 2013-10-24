using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Telerik.TeamPulse.Sdk.Common;
using Telerik.TeamPulse.Sdk.Models;
using System.Collections.Generic;

namespace Telerik.TeamPulse.Sdk
{
    public class UsersClient : ApiClientBase
    {
        public UsersClient(string siteUrl, string refreshToken, string username, string password) : 
            base(siteUrl, refreshToken, username, password)
        {
        }

        public UsersClient(string siteUrl, string accessToken) :
            base(siteUrl, accessToken)
        {
        }

        /// <summary>
        /// Get a user by ID
        /// </summary>       
        /// <param name="id">The ID of the user</param>
        /// <returns></returns>
        public User Get(int id)
        {
            return this.Get<User>(string.Format(ApiUrls.Users.GetOne, id));
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <param></param>
        /// <returns>Array of all users</returns>
        public ApiCollection<User> GetAll()
        {
            return this.Get<ApiCollection<User>>(ApiUrls.Users.GetAll);
        }

        /// <summary>
        /// Get all users with filter and sort
        /// </summary>
        /// <param name="oDataOptions">oData formatted query string</param>
        /// <returns>The requested users.</returns>
        public ApiCollection<User> GetAll(string oDataOptions)
        {
            return this.Get<ApiCollection<User>>(string.Format("{0}?{1}", ApiUrls.Users.GetAll, oDataOptions.TrimStart('?')));
        }

        /// <summary>
        /// Get current user
        /// </summary>        
        /// <returns>Array of all projects</returns>
        public User GetCurrent()
        {
            return this.Get<User>(ApiUrls.Users.GetCurrent);
        }
    }
}
