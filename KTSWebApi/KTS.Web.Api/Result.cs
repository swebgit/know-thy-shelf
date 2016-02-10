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
    }

    public class Result<T> : Result
    {
        [JsonProperty(PropertyName = "data")]
        public T Data { get; set; }        

        public Result(T data)
        {
            this.Data = data;
            this.Success = true;
            this.Error = null;
        }

        public Result(T data, bool success)
        {
            this.Data = data;
            this.Success = success;
            this.Error = null;
        }

        public Result(T data, string error)
        {
            this.Data = data;
            this.Success = false;
            this.Error = error;
        }
    }
}