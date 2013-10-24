using System.Net;
using System.Net.Http;
using System.Text;

namespace Telerik.TeamPulse.Sdk.Common
{
    public class RawRestClient
    {
        private string accessToken;
        private string siteUrl;

        public void Init(string siteUrl, string refreshToken, string username, string password)
        {
            this.siteUrl = siteUrl;
            var auth = new AuthenticationHelper(siteUrl, refreshToken, username, password);
            this.accessToken = auth.Authenticate();
        }

        public void Init(string siteUrl)
        {
            this.siteUrl = siteUrl;
        }

        public void Init(string siteUrl, string accessToken)
        {
            this.siteUrl = siteUrl;
            this.accessToken = accessToken;
        }

        public HttpResponseMessage Get(string url)
        {
            using (var httpClient = NewHttpClient())
            {
                var response = httpClient.GetAsync(this.siteUrl + "/" + url).Result;
                return response;
            }
        }

        public HttpResponseMessage Post(string url, string content)
        {
            using (var httpClient = NewHttpClient())
            {
                var requestContent = new StringContent(content, Encoding.UTF8, "application/json");

                var responseResult = httpClient.PostAsync(this.siteUrl + "/" + url, requestContent).Result;
                return responseResult;
            }
        }

        public HttpResponseMessage Put(string url, string content)
        {
            using (var httpClient = NewHttpClient())
            {
                var requestContent = new StringContent(content, Encoding.UTF8, "application/json");
                var responseResult = httpClient.PutAsync(this.siteUrl + "/" + url, requestContent).Result;
                return responseResult;
            }
        }

        public HttpResponseMessage Delete(string url)
        {
            using (var httpClient = NewHttpClient())
            {
                var responseResult = httpClient.DeleteAsync(this.siteUrl + "/" + url).Result;
                return responseResult;
            }
        }

        public HttpStatusCode GetResponseStatusCode(string url, bool skipAuthorization)
        {
            using (var httpClient = NewHttpClient(skipAuthorization))
            {
                var response = httpClient.GetAsync(this.siteUrl + "/" + url);
                return response.Result.StatusCode;
            }
        }

        public string GetAccessToken()
        {
            return this.accessToken;
        }

        public string CreateUrl(string url)
        {
            return string.Format("{0}/{1}", this.siteUrl.TrimEnd('/'), url.TrimStart('/'));
        }

        private HttpClient NewHttpClient(bool skipAuthorization = false)
        {
            var h = new WebRequestHandler();

            var client = new HttpClient(h);
            if (!skipAuthorization)
            {
                client.DefaultRequestHeaders.Add("Authorization", "WRAP access_token=" + this.accessToken);
            }

            return client;
        }

        
    }
}
