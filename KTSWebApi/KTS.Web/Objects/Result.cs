using KTS.Web.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTS.Web.Objects
{
    public class Result
    {
        [JsonProperty(PropertyName = "error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "resultCode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ResultCode ResultCode { get; set; }
        
        public Result(ResultCode resultCode = ResultCode.Failed)
        {
            this.ResultCode = resultCode;
            this.Error = null;
        }

        public Result(string error)
        {
            this.ResultCode = ResultCode.Failed;
            this.Error = error;
        }
        
        public Result(string expectedType, string actualType)
        {
            this.ResultCode = ResultCode.Failed;
            this.Error = $"Incorrect object type. Expected Type = {expectedType}. Actual Type = {actualType}";
        }
    }

    public class Result<T> : Result where T : class
    {
        [JsonProperty(PropertyName = "data")]
        public T Data { get; set; }

        public Result() : base()
        {
        }
        
        public Result(T data, ResultCode resultCode = ResultCode.Ok) : base(resultCode)
        {
            this.Data = data;
        }

        public Result(Result result)
        {
            this.ResultCode = result.ResultCode;
            this.Error = result.Error;
        }

        public Result(string error) : base(error)
        {

        }

        public Result(string expectedType, string actualType) : base(expectedType, actualType)
        {

        }
    }
}
