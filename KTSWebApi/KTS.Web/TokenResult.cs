using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTS.Web
{
    public class TokenResult
    {
        public string Token { get; set; }

        public TokenResult() { }
        public TokenResult(string token)
        {
            this.Token = token;
        }
    }
}
