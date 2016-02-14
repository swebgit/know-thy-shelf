using KTS.Web.Objects;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace KTS.Web.Interfaces
{
    public interface ISearchClient
    {
        Task<bool> CreateOrUpdateBookIndexAsync(DatabaseJObject book);

        Task<bool> DeleteBookIndexAsync(int id);
    }
}
