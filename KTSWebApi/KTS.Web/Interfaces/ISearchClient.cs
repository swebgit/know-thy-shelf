using KTS.Web.Objects;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KTS.Web.Interfaces
{
    public interface ISearchClient
    {
        Task<bool> CreateOrUpdateBookIndexAsync(DatabaseJObject book);

        Task<bool> DeleteBookIndexAsync(int id);

        Task<JToken> GetBooksAsync(string searchString, int pageNumber, int pageSize);
    }
}
