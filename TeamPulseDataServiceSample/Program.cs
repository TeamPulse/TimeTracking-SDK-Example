using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using TeamPulse.Integration;
using TeamPulseDataServiceSample.TeamPulse;

namespace TeamPulseDataServiceSample
{
    class Program
    {
        // Note: In order for forms auth to work, the JSON authentication service must be enabled on the server.
        // You can enable it by adding (un-commenting) the following in the TeamPulse IIS Web.config:
        // <system.web.extensions>
        //     <scripting>
        //         <webServices>
        //             <authenticationService enabled="true" requireSSL="false"/>
        //         </webServices> 
        //     </scripting>
        // </system.web.extensions>
        
        // CHANGE THIS URL TO A SERVICE THAT YOU'RE TESTING AGAINST, YOU WILL NEED TO DO THE SAME FOR THE SERVICE REFERENCE AND UPDATE IT
        const string teamPulseRootUrl = "http://localhost/TeamPulse";

        // CHANGE THESE CREDENTIALS TO VALID ONES IN YOUR ENVIRONMENT
        const string username = "superuser";
        const string password = "P@ssw0rd";
        const string domain = null;
        const bool useDefaultWindowsCredentials = true;
        const bool useWindowsAuth = false;

        static TitanDataContext dataContext;
        static bool gotStoryAcceptanceCritierias;
        static bool gotProblems;
        static List<Problem> problems = new List<Problem>();

        static string accessToken;

        static void Main(string[] args)
        {
            var authenticationHelper = new AuthenticationHelper(teamPulseRootUrl, null, true, username, password);
            accessToken = authenticationHelper.Authenticate();
           
            var serviceUrl = teamPulseRootUrl + "/Services/DataService.svc";

            // Instantiate an instance of the data service context
            dataContext = new TitanDataContext(new Uri(serviceUrl, UriKind.RelativeOrAbsolute));

            // Connect a handler to the SendingRequest event that will attach our auth cookie to the request
            dataContext.SendingRequest += BeforeSendingRequest;
   
            // Retrieve some information from the data service
            GetStoryAcceptanceCriterias(1);
            GetProblemAndProblemAcceptanceCriteria(1);

            // Execution needs to pause to let the requests and responses occur
            while (!gotProblems && !gotStoryAcceptanceCritierias)
            {
                System.Threading.Thread.Sleep(1000);
            }

            // Update value on a problem item
            UpdateAProblem();

            // Create a new story
            CreateAStory(1, 1, 1);

            CreateABug(1, 1, 1);
            
            Console.WriteLine("All done.  Press any key to exit.");
            Console.ReadKey();
        }
  
        /// <summary>
        /// Attaches a valid authentication cookie to each data service request.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private static void BeforeSendingRequest(object sender, SendingRequestEventArgs e)
        {
            e.RequestHeaders.Add("Authorization", "WRAP access_token=" + accessToken);
        }

        /// <summary>
        /// Example of a query to the TeamPulse data service.
        /// </summary>
        /// <remarks>
        /// Queries story acceptance criteria where the project id is X and  
        /// expand to eager load related story.  Only get the first 5 records.        /// </remarks>
        private static void GetStoryAcceptanceCriterias(int projectID)
        {

            string uri = string.Format("StoryAcceptanceCriterias?$filter=ProjectID eq {0}&$expand=Story&$top=5", projectID);
            dataContext.BeginExecute<StoryAcceptanceCriteria>(new Uri(uri, UriKind.Relative), ar =>
            {
                foreach (StoryAcceptanceCriteria storyAcceptanceCriteria in dataContext.EndExecute<StoryAcceptanceCriteria>(ar))
                {
                    Console.WriteLine("Story acceptance criteria " + storyAcceptanceCriteria.Description + " retrieved with story " +
                        storyAcceptanceCriteria.Story.Name);
                }
                gotStoryAcceptanceCritierias = true;
            },
            null);
        }

        /// <summary>
        /// Example of a batch query to the TeamPulse data service.
        /// </summary>
        /// <remarks>
        /// Queries problem with filter.
        /// Within the same query also gets problem acceptance criteria based on project id.
        /// </remarks>
        private static void GetProblemAndProblemAcceptanceCriteria(int projectID)
        {
            var problemsRequest = new DataServiceRequest<Problem>(new Uri(string.Format("Problems?$filter=ProjectID eq {0}", projectID), UriKind.Relative));
            var acceptanceCriteriaRequest = new DataServiceRequest<ProblemAcceptanceCriteria>(new Uri(string.Format("ProblemAcceptanceCriterias?$filter=ProjectID eq {0}", projectID), UriKind.Relative));
            dataContext.BeginExecuteBatch(ar =>
            {
                var batchOperationResponse = dataContext.EndExecuteBatch(ar);

                foreach (QueryOperationResponse qoResponse in batchOperationResponse)
                {
                    if (qoResponse.Error != null)
                        throw qoResponse.Error;

                    if (qoResponse is QueryOperationResponse<Problem>)
                    {
                        foreach (Problem problem in ((QueryOperationResponse<Problem>)qoResponse))
                        {
                            problems.Add(problem);
                            Console.WriteLine("Retrieved problem " + problem.Name);
                        }
                    }
                    else if (qoResponse is QueryOperationResponse<ProblemAcceptanceCriteria>)
                    {
                        foreach (ProblemAcceptanceCriteria problemAcceptanceCriteria in ((QueryOperationResponse<ProblemAcceptanceCriteria>)qoResponse))
                        {
                            Console.WriteLine("Retrieved problem acceptance criteria" + problemAcceptanceCriteria.DescriptionPlainText);
                        }
                    }
                }

                gotProblems = true;
            },
        gotProblems,
        problemsRequest,
        acceptanceCriteriaRequest);
        }
       
        /// <summary>
        /// Example of a updating an entity and saving the changes back.
        /// </summary>
        /// <remarks>
        /// Make a change to the entity, mark it as dirty with the UpdateObject method and then save the changes back.
        /// </remarks>
        private static void UpdateAProblem()
        {
            var problemToUpdate = problems[0];
            problemToUpdate.Estimate = problemToUpdate.Estimate + 10;

            Console.WriteLine(string.Format("Updated problem {0} with an estimate of {1}", problemToUpdate.ProblemID, problemToUpdate.Estimate));
            
            // Mark object as dirty
            dataContext.UpdateObject(problemToUpdate);

            dataContext.SaveChanges(); // This can also be an asynchronous call using BeginSaveChanges

            Console.WriteLine("Changes saved");
        }

        /// <summary>
        /// Example of creating a new story
        /// </summary>
        /// <param name="projectId">The id of the project this story will be a part of.</param>
        /// <param name="iterationId">The id of the iteration this story will be in.</param>
        /// <param name="areaId">The id of the area that this story will be in.</param>
        /// <remarks>
        /// The rich description is stored in a separate table and is therefore a different object in the
        /// model. This should always be created when creating a story.
        /// </remarks>
        private static void CreateAStory(int projectId, int iterationId, int areaId)
        {
            var newStory = new Story
            {
                ProjectID = projectId, 
                Name = "Test story", // The title of the story
                IterationID = iterationId,
                AreaID = areaId, 
                Status = "Not Done", // The initial status of the story. Can be derived from entity definition in project settings xml.
                VersionNumber = 1,
                Description = "As a <type of user> I want <some goal> so that <some reason>.",
                CreatedBy = "ausername", // a valid teampulse username
                CreatedDateUtc = DateTime.UtcNow,
                CreatedSystemID = 0, // 0 = TeamPulse, 1 = TFS, 2 = Notification Server, 3 = Feedback Portal
                LastModifiedBy = "ausername",
                LastModifiedDateUtc = DateTime.UtcNow,
                LastModifiedSystemID = 0
            };

            dataContext.AddToStories(newStory);

            dataContext.SaveChanges();

            Console.WriteLine("Story created");
        }

        /// <summary>
        /// Example of creating a new story
        /// </summary>
        /// <param name="projectId">The id of the project this story will be a part of.</param>
        /// <param name="iterationId">The id of the iteration this story will be in.</param>
        /// <param name="areaId">The id of the area that this story will be in.</param>
        /// <remarks>
        /// The rich description is stored in a separate table and is therefore a different object in the
        /// model. This should always be created when creating a story.
        /// </remarks>
        private static void CreateABug(int projectId, int iterationId, int areaId)
        {
            var newBug = new Problem
            {
                ProblemType = 0,
                ProjectID = projectId,
                Name = "Test story", // The title of the story
                IterationID = iterationId,
                AreaID = areaId,
                Status = "Not Done", // The initial status of the story. Can be derived from entity definition in project settings xml.
                VersionNumber = 1,
                DescriptionPlainText = "As a <type of user> I want <some goal> so that <some reason>.",
                StepsToReproducePlainText = "1. Step 1\r\n2.Step 2\r\n3.Step 3",
                ResolutionPlainText = "Simple resolution",
                BenefitPlainText = "Benefits",
                CreatedBy = "ausername", // a valid teampulse username
                CreatedDateUtc = DateTime.UtcNow,
                CreatedSystemID = 0, // 0 = TeamPulse, 1 = TFS, 2 = Notification Server, 3 = Feedback Portal
                LastModifiedBy = "ausername",
                LastModifiedDateUtc = DateTime.UtcNow,
                LastModifiedSystemID = 0
            };

            dataContext.AddToProblems(newBug);

            dataContext.SaveChanges();

            Console.WriteLine("Bug created");
        }
    }
}