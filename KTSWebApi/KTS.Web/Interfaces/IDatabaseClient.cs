using KTS.Web.Objects;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace KTS.Web.Interfaces
{
    public interface IDatabaseClient
    {
        Task<DatabaseJObject> GetBookAsync(int id);

        Task<DatabaseJObject> CreateOrUpdateBookAsync(DatabaseJObject book);

        Task<bool> DeleteBookAsync(int id);
    }
}
