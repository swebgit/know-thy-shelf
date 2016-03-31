using KTS.Web.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTS.Web.Interfaces
{
    public interface IKTSApiClient
    {
        Task<Result<TokenResult>> LogOn(Credentials credentials);
    }
}
