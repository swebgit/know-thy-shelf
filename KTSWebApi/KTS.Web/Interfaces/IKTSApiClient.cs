using KTS.Web.Objects;
using Newtonsoft.Json.Linq;
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

        Task<Result<List<Book>>> GetBooks(string searchString, int pageNumber, int pageSize, string authToken);
    }
}
