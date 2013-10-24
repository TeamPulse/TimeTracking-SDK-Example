using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Telerik.TeamPulse.Sdk.Common
{
    public class AuthenticationHelper
    {
        public string TeamPulseUrl { get; private set; }
        
        private string domain;
        private readonly string username;
        private readonly string password;

        private string RestrictedUrl
        {
            get
            {
                return TeamPulseUrl + "/Authentication/Restrict";
            }
        }
        
        public string AccessToken { get; private set; }

        public string RefreshToken { get; private set; }

        public AuthenticationHelper(string teamPulseUrl, string refreshToken, string username, string password)
            : this(teamPulseUrl, refreshToken, username, password, null)
        {

        }

        public AuthenticationHelper(string teamPulseUrl, string refreshToken, string username, string password, string domain)
        {
            this.RefreshToken = refreshToken;
            this.password = password;
            this.username = username;
            this.domain = domain;            
            this.TeamPulseUrl = teamPulseUrl;
        }

        public string Authenticate()
        {
            if (!string.IsNullOrEmpty(AccessToken))
                return AccessToken;

            WebHeaderCollection responseHeaders;
            HttpStatusCode responseCode = TryToAccessTeamPulse(RestrictedUrl, out responseHeaders);

            switch (responseCode)
            {
                case HttpStatusCode.Unauthorized:
                    {
                        string location, clientId;
                        GetLocationAndClientId(responseHeaders, out location, out clientId);
                        if (String.IsNullOrEmpty(location) || String.IsNullOrEmpty(clientId))
                        {
                            throw new ApplicationException("Could not determine Security Token Service.");
                        }

                        if (!String.IsNullOrEmpty(RefreshToken))
                        {
                            AccessToken = AcquireAccessTokenFromRefreshToken(location, clientId, RefreshToken);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(domain))
                            {
                                string refreshToken;
                                AccessToken = AcquireAccessTokenWithPassword(location, clientId, username, password, out refreshToken);
                                RefreshToken = refreshToken;
                            }
                            else
                            {
                                string refreshToken;
                                var cookie = GetWindowsCredsAuthCookie(TeamPulseUrl, true);
                                AccessToken = AcquireAccessTokenFromTheCookie(location, clientId, cookie, out refreshToken);
                                RefreshToken = refreshToken;
                            }
                        }
                        return AccessToken;
                    }
                case HttpStatusCode.OK:
                    throw new ApplicationException("User is already authenticated.");
                case HttpStatusCode.InternalServerError:
                    throw new ApplicationException("It seems that the relying party and/or the STS applications are not running.");
                default:
                    throw new InvalidOperationException("Invalid response.");
            }
        }

        private static HttpStatusCode TryToAccessTeamPulse(string teamPulseUrl, out WebHeaderCollection responseHeaders)
        {
            string body;
            
                NameValueCollection requestHeaders = new NameValueCollection();
                requestHeaders["Authorization"] = "WRAP";

                var responseCode = Request(teamPulseUrl, out body, out responseHeaders, requestHeaders: requestHeaders);

                return responseCode;
        }

        private static string AcquireAccessTokenFromRefreshToken(string location, string clientId, string refreshToken)
        {
            string body;
            WebHeaderCollection responseHeaders;
            var postData = new NameValueCollection();
            postData["wrap_client_id"] = clientId;
            postData["wrap_refresh_token"] = refreshToken;
            var bytes = Encoding.UTF8.GetBytes(postData.ToQueryString(false, true));

            var responseCode = Request(location, out body, out responseHeaders, bytes, "POST", "application/x-www-form-urlencoded");

            string accessToken = null;

            switch (responseCode)
            {
                case HttpStatusCode.OK:
                    {
                        var response = HttpUtility.ParseQueryString(body);
                        accessToken = response["wrap_access_token"];
                        refreshToken = response["wrap_refresh_token"];

                        return accessToken;
                    }
                case HttpStatusCode.Unauthorized:
                    throw new ApplicationException("The refresh token is invalid");
                default:
                    throw new ApplicationException("The server returned response code {0} when try to get access token " + responseCode);
            }
        }

        private static string AcquireAccessTokenWithPassword(string location, string clientId, string username, string password, out string refreshToken)
        {
            refreshToken = null;

            string body;
            WebHeaderCollection responseHeaders;
            var postData = new NameValueCollection();
            postData["wrap_client_id"] = clientId;
            postData["wrap_username"] = username;
            postData["wrap_password"] = password;
            var bytes = Encoding.UTF8.GetBytes(postData.ToQueryString(false));
            var responseCode = Request(location, out body, out responseHeaders, bytes, "POST", "application/x-www-form-urlencoded");

            string accessToken = null;
            switch (responseCode)
            {
                case HttpStatusCode.OK:
                    {
                        postData = HttpUtility.ParseQueryString(body);
                        accessToken = postData["wrap_access_token"];
                        refreshToken = postData["wrap_refresh_token"];
                        return accessToken;
                    }

                case HttpStatusCode.Unauthorized:
                    throw new ApplicationException("Invalid username or password.");
                default:
                    throw new ApplicationException(string.Format("The server returned response code {0}.", responseCode));
            }
        }



        private static void GetLocationAndClientId(NameValueCollection responseHeaders, out string location, out string clientId)
        {
            string wrapHeader = null;
            var authHeaders = responseHeaders.GetValues("WWW-Authenticate");
            foreach (var hdr in authHeaders)
            {
                if (hdr.StartsWith("WRAP "))
                {
                    wrapHeader = hdr.Substring(5);
                    break;
                }
            }

            if (wrapHeader != null)
            {
                var pattern = Extensions.NameValueRegexPatterns.EqualitySignAndQuotationMarks;
                var coll = Extensions.ParseNameValueCollection(wrapHeader, pattern);

                location = coll["location"];
                clientId = coll["client_id"];
            }
            else
            {
                location = null;
                clientId = null;
            }
        }

        /// <summary>
        /// Makes a synchronous HTTP request to the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="responseBody">The response body.</param>
        /// <param name="responseHeaders">The response headers.</param>
        /// <param name="data">The data.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="requestHeaders">The request headers.</param>
        /// <returns></returns>
        public static HttpStatusCode Request(string url, out string responseBody, out WebHeaderCollection responseHeaders, byte[] data = null, string httpMethod = "GET", string contentType = "", NameValueCollection requestHeaders = null, Cookie cookie = null)
        {
            Uri uri = new Uri(url);

            // Create and set the request object
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.AllowAutoRedirect = false;
            request.Method = httpMethod;
            request.ContentType = contentType;
            request.CookieContainer = new CookieContainer();

            if (requestHeaders != null)
                request.Headers.Add(requestHeaders);

            if (cookie != null)
                request.CookieContainer.Add(uri, cookie);

            if (data != null)
            {
                request.ContentLength = data.Length;
                // Send the data to the request stream
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(data, 0, data.Length);
                }
            }

            // Invoke the method and return the response.
            HttpStatusCode statusCode;
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    response = (HttpWebResponse)ex.Response;
                }
            }
            finally
            {
                if (response != null)
                {
                    // Read the response
                    using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseBody = reader.ReadToEnd();
                    }
                    responseHeaders = response.Headers;
                    statusCode = response.StatusCode;
                    response.Close();
                }
                else
                {
                    statusCode = HttpStatusCode.InternalServerError;
                    responseBody = "";
                    responseHeaders = null;
                }
            }
            return statusCode;
        }

        /// <summary>
        /// Gets an authentication cookie by calling the WinLogin page
        /// <param name="useDefaultCreds">Use Default Windows Credentials of the current windows user. Default is true.</param>
        /// <param name="username">The Windows username to authenticate with. Not required if using Default Credentials.</param>
        /// <param name="password">The Windows password to authenticate with. Not required if using Default Credentials.</param>
        /// <param name="domain">The Windows domain to authenticate with. Not required, current domain will be used if not provided.</param>
        /// <returns>The contents of the authentication cookie.</returns>
        private static Cookie GetWindowsCredsAuthCookie(string teamPulseRootUrl, bool useDefaultCreds = true, string username = null, string password = null, string domain = null)
        {
            string winLoginPageAddress = teamPulseRootUrl + "/WinLogin/Login.aspx?ReturnUrl=0";

            var request = CreateWindowsCredsAuthRequest(winLoginPageAddress, useDefaultCreds, username, password, domain);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.Headers["Set-Cookie"] != null)
            {
                var cookies = response.Headers["Set-Cookie"].Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string c in cookies)
                {
                    //if (!string.IsNullOrWhiteSpace(c))
                    if (!string.IsNullOrEmpty(c))
                    {
                        var trimmedc = c.Trim();
                        if (trimmedc.StartsWith(".ASPXAUTH="))
                        {
                            var authenticationCookie = trimmedc.Split('=');


                            return new Cookie(authenticationCookie[0], authenticationCookie[1]);
                        }
                    }
                }

                throw new ApplicationException("Invalid credentials");
            }

            throw new Exception("Server didn't send any cookies");
        }

        private static string AcquireAccessTokenFromTheCookie(string location, string clientId, Cookie cookie, out string refreshToken)
        {
            refreshToken = null;
            string accessToken = null;

            var queryString = new NameValueCollection();
            queryString["wrap_client_id"] = clientId;
            var url = location + queryString.ToQueryString(true, true);


            string body;
            WebHeaderCollection headers;
            var statusCode = Request(url, out body, out headers, cookie: cookie);
            var match = System.Text.RegularExpressions.Regex.Match(body, @"<title>(?<code>.+)</title>");
            if (match.Success)
            {
                string verificationCode = ReadVerificationCodeFromTitle(match.Groups["code"].Value);
                accessToken = GetTheAccessCodeForVerificationCode(location, clientId, verificationCode, ref refreshToken);
            }
            else
            {
                throw new ApplicationException();
            }

            return accessToken;
        }

        private static string ReadVerificationCodeFromTitle(string title)
        {
            var length = "Authentication Delegation,".Length;
            var response = HttpUtility.ParseQueryString(title.Substring(length).Trim());
            var code = response["code"];
            if (code == "user_denied")
            {
                throw new ApplicationException("Authentication denied by user.");
            }

            return code;
        }

        private static string GetTheAccessCodeForVerificationCode(string location, string clientId, string verificationCode, ref string refreshToken)
        {
            string body;
            WebHeaderCollection responseHeaders;
            var postData = new NameValueCollection();
            postData["wrap_client_id"] = clientId;
            postData["wrap_verification_code"] = verificationCode;
            var bytes = Encoding.UTF8.GetBytes(postData.ToQueryString(false));
            var responseCode = Request(location, out body, out responseHeaders, bytes, "POST", "application/x-www-form-urlencoded");

            if (responseCode == HttpStatusCode.OK)
            {
                var response = HttpUtility.ParseQueryString(body);
                refreshToken = response["wrap_refresh_token"];
                var accessToken = response["wrap_access_token"];

                return accessToken;
            }
            else
            {
                throw new ApplicationException("The server returned response: " + responseCode);
            }
        }

        /// <summary>
        /// Creates the request to be sent to the login service end point.
        /// </summary>
        /// <param name="pageAddress">The Windows Login Page address.</param>
        /// <param name="useDefaultCreds">Use Default Windows Credentials of the current windows user. Default is true.</param>
        /// <param name="username">The Windows username to authenticate with. Not required if using Default Credentials.</param>
        /// <param name="password">The Windows password to authenticate with. Not required if using Default Credentials.</param>
        /// <param name="domain">The Windows domain to authenticate with. Not required, current domain will be used if not provided.</param>
        /// <returns>A WebRequest object.</returns>
        private static WebRequest CreateWindowsCredsAuthRequest(string pageAddress, bool useDefaultCreds = true, string username = null, string password = null, string domain = null)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(pageAddress);

            request.Method = "GET";
            request.UseDefaultCredentials = useDefaultCreds;
            request.PreAuthenticate = true;
            request.AllowAutoRedirect = false;

            if (useDefaultCreds)
            {
                request.Credentials = CredentialCache.DefaultCredentials;
            }
            else
            {
                if (username != null && password != null)
                {
                    if (domain == null)
                        request.Credentials = new NetworkCredential(username, password);
                    else
                        request.Credentials = new NetworkCredential(username, password, domain);
                }
                else
                {
                    if (username == null)
                        throw new ArgumentNullException("username");
                    else
                        throw new ArgumentNullException("password");
                }
            }

            return request;
        }
    }
}
