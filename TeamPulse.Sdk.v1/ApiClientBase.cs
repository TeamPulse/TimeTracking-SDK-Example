using System.Web.Http;
using Telerik.TeamPulse.Sdk.Common;
using Telerik.TeamPulse.Sdk.Models;

namespace Telerik.TeamPulse.Sdk
{
    public class ApiClientBase
    {
        protected RawRestClient apiClient;
        public ApiClientBase(string siteUrl, string refreshToken, string username, string password)
        {
            apiClient = new RawRestClient();
            apiClient.Init(siteUrl, refreshToken, username, password);
        }

        public ApiClientBase(string siteUrl, string accessToken)
        {
            apiClient = new RawRestClient();
            apiClient.Init(siteUrl, accessToken);
        }

        protected T Post<T>(string url, string request)
        {
            var response = apiClient.Post(url, request);
            var responseContent = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpResponseException(response);
            }

            return SerializationHelper.DeserializeFromJson<T>(responseContent);
        }

        protected T Put<T>(string url, string request)
        {
            var response = apiClient.Put(url, request);
            var responseContent = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpResponseException(response);
            }

            return SerializationHelper.DeserializeFromJson<T>(responseContent);
        }

        public void Delete(string url)
        {
            var response = apiClient.Delete(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpResponseException(response);
            }
        }

        protected T Get<T>(string url)
        {
            var response = apiClient.Get(url);
            var responseContent = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpResponseException(response);
            }

            return SerializationHelper.DeserializeFromJson<T>(responseContent);
        }
    }
}
