using KTS.Web.Enums;
using Newtonsoft.Json;

namespace KTS.Web.Api
{
    public class Result
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }

        public Result(bool success = true)
        {
            this.Success = success;
            this.Error = null;
        }

        public Result(string error)
        {
            this.Success = false;
            this.Error = error;
        }

        public Result(string expectedType, string actualType)
        {
            this.Success = false;
            this.Error = $"Incorrect object type. Expected Type = {expectedType}. Actual Type = {actualType}";
        }
    }

    public class ApiResult<T> : Result
    {
        [JsonProperty(PropertyName = "data")]
        public T Data { get; set; }        

        public ApiResult(T data)
        {
            this.Data = data;
            this.Success = true;
            this.Error = null;
        }

        public ApiResult(T data, bool success)
        {
            this.Data = data;
            this.Success = success;
            this.Error = null;
        }

        public ApiResult(T data, string error)
        {
            this.Data = data;
            this.Success = false;
            this.Error = error;
        }
    }
}