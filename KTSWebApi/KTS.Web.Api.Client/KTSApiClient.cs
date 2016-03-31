using KTS.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KTS.Web.Objects;
using System.Net.Http;
using System.Configuration;
using System.Net;
using Newtonsoft.Json;

namespace KTS.Web.Api.Client
{
    public class KTSApiClient : IKTSApiClient
    {
#if DEBUG
        private const string API_URL = @"http://localhost:32015";
#else
        private const string API_URL = @"https://api.knowthyshelf.com";
#endif

        private const string LOGIN_CONTROLLER = "api/auth";

        private const string AUTH_TOKEN_HEADER = "auth-token";
        private const string API_KEY_HEADER = "api-key";
        private static readonly string API_KEY_VALUE = ConfigurationManager.AppSettings["SharedApiKey"];

        private static readonly TimeSpan DEFAULT_TIMEOUT = TimeSpan.FromSeconds(30);

        private readonly HttpClient httpClient;

        public KTSApiClient()
        {
            this.httpClient = new HttpClient();
        }

        public async Task<Result<TokenResult>> LogOn(Credentials credentials)
        {
            var json = JsonConvert.SerializeObject(credentials);
            var response = await this.SendHttpRequestAsync(HttpMethod.Post, LOGIN_CONTROLLER, null, json);

            if (response.IsSuccessStatusCode)
            {
                using (var content = response.Content)
                {
                    var contentString = await content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(contentString))
                    {
                        return JsonConvert.DeserializeObject<Result<TokenResult>>(contentString);
                    }
                }
            }

            return new Result<TokenResult>();
        }

        private async Task<HttpResponseMessage> SendHttpRequestAsync(HttpMethod method, string route, string authToken = null, string jsonContent = null, TimeSpan? timeout = null)
        {
            var request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(string.Format("{0}/{1}", API_URL, route)),
            };

            // add the api key header, required for talking with the API
            request.Headers.Add(API_KEY_HEADER, API_KEY_VALUE);
            
            // if the auth token was specified, add it to the request headers
            if (!string.IsNullOrEmpty(authToken))
            {
                request.Headers.Add(AUTH_TOKEN_HEADER, authToken);
            }

            // if json content was specified, add it to the body of the message
            if (!string.IsNullOrEmpty(jsonContent))
            {
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            }

            this.httpClient.Timeout = timeout ?? DEFAULT_TIMEOUT;

            try
            {
                var response = await this.httpClient.SendAsync(request);

                return response;
            }
            catch (TaskCanceledException)
            {
                // TaskCanceled Exception is thrown if the timeout is exceeded
                return new HttpResponseMessage(HttpStatusCode.RequestTimeout);
            }
            catch (Exception)
            {
                // if we don't know what caused the exception, returned the Unused code
                return new HttpResponseMessage(HttpStatusCode.Unused);
            }
        }
    }
}