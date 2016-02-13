using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace KTS.Web.Api.Interfaces
{
    public interface IDatabaseClient
    {
        Task<JObject> GetBookAsync(int id);

        Task<int> CreateOrUpdateBookAsync(JObject book);

        Task<bool> DeleteBookAsync(int id);
    }
}
