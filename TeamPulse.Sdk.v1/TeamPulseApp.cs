using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.TeamPulse.Sdk.Common;

namespace Telerik.TeamPulse.Sdk
{  
    public class TeamPulseApp
    {
        private string accessToken;
        private TeamPulseAppSettings settings;

        public TeamPulseApp(TeamPulseAppSettings settings)
        {
            this.settings = settings; 
        }

        public void Login()
        {
            var auth = new AuthenticationHelper(this.settings.SiteUrl, null, this.settings.Username, this.settings.Password, this.settings.Domain);
            this.accessToken = auth.Authenticate();
        }

        private void AssertAccessToken()
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                this.Login();
            }
        }

        public AcceptanceCriteriaClient AcceptanceCriteria
        {
            get
            {
                this.AssertAccessToken();
                return new AcceptanceCriteriaClient(this.settings.SiteUrl, this.accessToken);
            }
        }

        public ActivitiesClient Activities
        {
            get
            {
                this.AssertAccessToken();
                return new ActivitiesClient(this.settings.SiteUrl, this.accessToken);
            }
        }

        public AreasClient Areas
        {
            get
            {
                this.AssertAccessToken();
                return new AreasClient(this.settings.SiteUrl, this.accessToken);
            }
        }
        
        public AttachmentsClient Attachments
        {
            get
            {
                this.AssertAccessToken();
                return new AttachmentsClient(this.settings.SiteUrl, this.accessToken);
            }
        }
                
        public CommentsClient Comments
        {
            get
            {
                this.AssertAccessToken();
                return new CommentsClient(this.settings.SiteUrl, this.accessToken);
            }
        }
        
        public IterationsClient Iterations
        {
            get
            {
                this.AssertAccessToken();
                return new IterationsClient(this.settings.SiteUrl, this.accessToken);
            }
        }

        public LinksClient Links
        {
            get
            {
                this.AssertAccessToken();
                return new LinksClient(this.settings.SiteUrl, this.accessToken);
            }
        }

        public ProjectsClient Projects
        {
            get
            {
                this.AssertAccessToken();
                return new ProjectsClient(this.settings.SiteUrl, this.accessToken);
            }
        }
        
        public TeamsClient Teams
        {
            get
            {
                this.AssertAccessToken();
                return new TeamsClient(this.settings.SiteUrl, this.accessToken);
            }
        }

        public TimeEntriesClient TimeEntries
        {
            get
            {
                this.AssertAccessToken();
                return new TimeEntriesClient(this.settings.SiteUrl, this.accessToken);
            }
        }

        public TimeEntryTypesClient TimeEntryTypes
        {
            get
            {
                this.AssertAccessToken();
                return new TimeEntryTypesClient(this.settings.SiteUrl, this.accessToken);
            }
        }

        public WorkItemsClient WorkItems
        {
            get
            {
                this.AssertAccessToken();
                return new WorkItemsClient(this.settings.SiteUrl, this.accessToken);
            }
        }

        public UsersClient Users
        {
            get
            {
                this.AssertAccessToken();
                return new UsersClient(this.settings.SiteUrl, this.accessToken);
            }
        }
    }
}
