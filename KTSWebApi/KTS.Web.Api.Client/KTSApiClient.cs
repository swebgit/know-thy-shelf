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
using Newtonsoft.Json.Linq;

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
        private const string BOOKS_CONTROLLER = "api/books";

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
            return await this.SendHttpRequestAsync<TokenResult>(HttpMethod.Post, LOGIN_CONTROLLER, null, json);
        }

        public async Task<Result<JObject>> GetBook(int id)
        {
            return await this.SendHttpRequestAsync<JObject>(HttpMethod.Get, $"{BOOKS_CONTROLLER}/{id}");
        }

        public async Task<Result<ObjectIdResult>> SaveBook(JObject bookData, string authToken)
        {
            return await this.SendHttpRequestAsync<ObjectIdResult>(HttpMethod.Post, BOOKS_CONTROLLER, authToken, bookData.ToString());
        }

        public async Task<Result<List<Book>>> GetBooks(string searchString, int pageNumber, int pageSize, string authToken)
        {
            var responseResult = await this.SendHttpRequestAsync<JToken>(HttpMethod.Get, $"{BOOKS_CONTROLLER}/{pageNumber}/{pageSize}/?searchString={searchString}", authToken);

            if (responseResult.ResultCode == Enums.ResultCode.Ok)
            {
                return new Result<List<Book>>(JsonConvert.DeserializeObject<List<Book>>(responseResult.Data.ToString()));
            }
            return new Result<List<Book>>(responseResult);
        }

        public async Task<Result> DeleteBook(int objectId, string authToken)
        {
            return await this.SendHttpRequestAsync(HttpMethod.Delete, $"{BOOKS_CONTROLLER}/{objectId}", authToken);
        }

        private async Task<Result<T>> DeserializeResponse<T>(HttpResponseMessage response) where T : class
        {
            if (response.IsSuccessStatusCode)
            {
                using (var content = response.Content)
                {
                    var contentString = await content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(contentString))
                    {
                        return JsonConvert.DeserializeObject<Result<T>>(contentString);
                    }
                }
            }

            return new Result<T>();
        } 

        private async Task<Result<T>> SendHttpRequestAsync<T>(HttpMethod method, string route, string authToken = null, string jsonContent = null, TimeSpan? timeout = null)
            where T : class
        {
            var response = await this.InternalSendHttpRequestAsync(method, route, authToken, jsonContent, timeout);

            if (response.IsSuccessStatusCode)
            {
                using (var content = response.Content)
                {
                    var contentString = await content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(contentString))
                    {
                        return JsonConvert.DeserializeObject<Result<T>>(contentString);
                    }
                }
            }

            return new Result<T>();
        }

        private async Task<Result> SendHttpRequestAsync(HttpMethod method, string route, string authToken = null, string jsonContent = null, TimeSpan? timeout = null)
        {
            var response = await this.InternalSendHttpRequestAsync(method, route, authToken, jsonContent, timeout);

            if (response.IsSuccessStatusCode)
            {
                using (var content = response.Content)
                {
                    var contentString = await content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(contentString))
                    {
                        return JsonConvert.DeserializeObject<Result>(contentString);
                    }
                }
            }

            return new Result();
        }

        private async Task<HttpResponseMessage> InternalSendHttpRequestAsync(HttpMethod method, string route, string authToken, string jsonContent, TimeSpan? timeout)
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

            HttpResponseMessage response;

            try
            {
                response = await this.httpClient.SendAsync(request);
            }
            catch (TaskCanceledException)
            {
                // TaskCanceled Exception is thrown if the timeout is exceeded
                response = new HttpResponseMessage(HttpStatusCode.RequestTimeout);
            }
            catch (Exception)
            {
                // if we don't know what caused the exception, returned the Unused code
                response = new HttpResponseMessage(HttpStatusCode.Unused);
            }

            return response;
        }
    }
}