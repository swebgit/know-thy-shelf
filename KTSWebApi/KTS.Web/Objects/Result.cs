using KTS.Web.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTS.Web.Objects
{
    public class Result<T> where T : new()
    {
        public ResultCode ResultCode { get; set; }
        public T Data { get; set; }

        public Result()
        {
            this.ResultCode = ResultCode.Failed;
            this.Data = new T();
        }
    }
}
