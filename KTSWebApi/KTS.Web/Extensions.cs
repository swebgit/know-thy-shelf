using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTS.Web
{
    public static class Extensions
    {
        public static T GetValue<T>(this JObject jObject, string key)
        {
            try
            {
                return jObject[key].ToObject<T>();
            }
            catch
            {
                return default(T);
            }
        }
    }
}
