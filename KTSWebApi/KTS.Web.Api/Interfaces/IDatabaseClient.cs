using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace KTS.Web.Api.Interfaces
{
    public interface IDatabaseClient
    {
        Task<JToken> GetBookAsync(int id);

        Task<int> CreateOrUpdateBookAsync(JToken book);

        Task<bool> DeleteBookAsync(int id);
    }
}
