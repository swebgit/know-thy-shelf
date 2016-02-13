using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace KTS.Web.Api.Interfaces
{
    public interface ISearchClient
    {
        Task<bool> CreateOrUpdateBookIndexAsync(JObject book, int id);

        Task<bool> DeleteBookIndexAsync(int id);
    }
}
