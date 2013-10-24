using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.TeamPulse.Sdk.Common;
using Telerik.TeamPulse.Sdk.Models;

namespace Telerik.TeamPulse.Sdk
{
    public class ActivitiesClient : ApiClientBase
    {
        public ActivitiesClient(string siteUrl, string refreshToken, string username, string password) : 
            base(siteUrl, refreshToken, username, password)
        {
        }

        public ActivitiesClient(string siteUrl, string accessToken) :
            base(siteUrl, accessToken)
        {
        }

        /// <summary>
        /// Get activities
        /// </summary>
        /// <returns>The requested activities</returns>
        public ApiCollection<Activity> Get()
        {
            return this.Get<ApiCollection<Activity>>(ApiUrls.Activities.Get);
        }

        /// <summary>
        /// Get activities with filter and sort
        /// </summary>
        /// <param name="oDataOptions">oData formatted query string</param>
        /// <returns>The requested activities</returns>
        public ApiCollection<Activity> Get(string oDataOptions)
        {
            return this.Get<ApiCollection<Activity>>(string.Format("{0}?{1}", ApiUrls.Activities.Get, oDataOptions.TrimStart('?')));
        }

        /// <summary>
        /// Get activities unread info
        /// </summary>
        /// <returns>Information about flagged and unflagged unread activities count</returns>
        public ActivityUnreadInfo GetUnreadInfo()
        {
            return this.Get<ActivityUnreadInfo>(ApiUrls.Activities.GetUnread);
        }
                
        /// <summary>
        /// Update an activity
        /// </summary>
        /// <param name="activity">The activity with the updated fields</param>
        /// <returns>The updated activity</returns>
        public Activity Update(Activity activity)
        {
            return this.Put<Activity>(string.Format(ApiUrls.Activities.Put, activity.id), SerializationHelper.SerializeToJson(activity));
        }

        /// <summary>
        /// Partial update of a activity
        /// </summary>
        /// <param name="activityId">The ID of the activity</param>
        /// <param name="activityValues">The fields which has to be updated</param>
        /// <returns>The updated activity</returns>
        public Activity Update(int activityId, Dictionary<string, object> activityValues)
        {
            return this.Put<Activity>(string.Format(ApiUrls.Activities.Put, activityId), SerializationHelper.SerializeToJson(activityValues));
        }

        public ActivitySharedMessage Share(ActivitySharedMessage message)
        {
            return this.Post<ActivitySharedMessage>(ApiUrls.Activities.PostShare, SerializationHelper.SerializeToJson(message));
        }
    }
}
