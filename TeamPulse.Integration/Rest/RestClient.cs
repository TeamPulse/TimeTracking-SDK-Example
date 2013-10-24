using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System;

namespace TeamPulse.Integration.Rest
{
    public class RestClient
    {
        protected readonly string _endpoint;
        protected readonly string accessToken;

        public RestClient(string endpoint, string accessToken)
        {
            _endpoint = endpoint;
            this.accessToken = accessToken;
        }

        public T Get<T>(int top, int skip)
        {
            return Get<T>(new Dictionary<string, object> { { "$top", top }, { "$skip", skip } });
        }

        public T Get<T>(IDictionary<string, object> parameters = null)
        {
            using (var httpClient = NewHttpClient())
            {
                var endpoint = CreateTheUrl(parameters);

                var response = httpClient.GetAsync(endpoint).Result;

                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
        }
    
        private string CreateTheUrl(IDictionary<string, object> parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return _endpoint;

            var endpoint = _endpoint + "?";

            var q = from p in parameters select string.Format("{0}={1}", p.Key, p.Value);

            endpoint += string.Join("&", q.ToArray());

            return endpoint;
        }

        public T Get<T>(int id)
        {
            using (var httpClient = NewHttpClient())
            {
                var response = httpClient.GetAsync(_endpoint + id).Result;

                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
        }

        public T Post<T>(T data)
        {
            using (var httpClient = NewHttpClient())
            {
                var content = JsonConvert.SerializeObject(data);
                var requestContent = new StringContent(content, Encoding.UTF8, "application/json");

                var responseResult = httpClient.PostAsync(_endpoint, requestContent).Result;
                var responseContent = responseResult.Content.ReadAsStringAsync().Result;
                if (!responseResult.IsSuccessStatusCode)
                {
                    throw new ApplicationException(responseContent);
                }

                return JsonConvert.DeserializeObject<T>(responseContent);
            }
        }

        public string Put<T>(int id, T data)
        {
            using (var httpClient = NewHttpClient())
            {
                var content = GetHttpContent<T>(data);

                var result = httpClient.PutAsync(_endpoint + id, content).Result;

                return result.Content.ReadAsStringAsync().Result;
            }
        }

        public string Delete(int id)
        {
            using (var httpClient = NewHttpClient())
            {
                var result = httpClient.DeleteAsync(_endpoint + id).Result;

                return result.Content.ToString();
            }
        }

        protected HttpContent GetHttpContent<T>(T data)
        {
            return new ObjectContent<T>(data, new JsonMediaTypeFormatter());
        }

        protected HttpClient NewHttpClient()
        {
            var h = new WebRequestHandler();
              
            //h.CookieContainer = new CookieContainer();
            //var splitedCookie = accessToken.Split('=', ';');
            //h.CookieContainer.Add(new Cookie(splitedCookie[0], splitedCookie[1], "/", new Uri(_endpoint).Host));
            var client = new HttpClient(h);
            client.DefaultRequestHeaders.Add("Authorization", "WRAP access_token=" + accessToken);

            return client; 
        }
    }
}
