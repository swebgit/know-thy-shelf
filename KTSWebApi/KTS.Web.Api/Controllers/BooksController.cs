using KTS.Web.Api.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace KTS.Web.Api.Controllers
{
    [RoutePrefix("api/books")]
    public class BooksController : ApiController
    {
        private IDatabaseClient databaseClient;
        private ISearchClient searchClient;

        public BooksController(IDatabaseClient databaseClient, ISearchClient searchClient)
        {
            this.databaseClient = databaseClient;
            this.searchClient = searchClient;
        }

        // GET: api/books/{id}
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetBook(int id)
        {
            try
            {
                var book = await this.databaseClient.GetBookAsync(id);
                return Ok(new Result<JObject>(book, book != null));
            }
            catch (Exception ex)
            {
                return Ok(new Result(ex.ToString()));
            }
        }

        // POST: api/books
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> CreateOrUpdateBook(JObject book)
        {
            try
            {
                var objectId = await this.databaseClient.CreateOrUpdateBookAsync(book);
                await this.searchClient.CreateOrUpdateBookIndexAsync(book, objectId);
                return Ok(objectId);
            }
            catch (Exception ex)
            {
                return Ok(new Result(ex.ToString()));
            }
        }

        // DELETE: api/books/{id}
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> DeleteBook(int id)
        {
            try
            {
                var result = await this.databaseClient.DeleteBookAsync(id);
                if (result)
                {
                    result &= await this.searchClient.DeleteBookIndexAsync(id);
                }
                return Ok(new Result(result));
            }
            catch (Exception ex)
            {
                return Ok(new Result(ex.ToString()));
            }
        }
    }
}
