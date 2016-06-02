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

        Task<Result<JObject>> GetBook(int id);

        Task<Result<ObjectIdResult>> SaveBook(JObject bookData, string authToken);

        Task<Result> DeleteBook(int objectId, string authToken);
    }
}
