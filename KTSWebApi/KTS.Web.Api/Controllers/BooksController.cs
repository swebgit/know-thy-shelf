using KTS.Web.Api.Attributes;
using KTS.Web.Api.Filters;
using KTS.Web.Enums;
using KTS.Web.Interfaces;
using KTS.Web.Objects;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace KTS.Web.Api.Controllers
{
    [RoutePrefix("api/books")]
    [SharedApiKeyFilter]
    public class BooksController : ClaimsEnabledController
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
                return Ok(new Result<JObject>(book.JObject, book != null ? ResultCode.Ok : ResultCode.Failed));
            }
            catch (Exception ex)
            {
                return Ok(new Result(ex.ToString()));
            }
        }

        // GET: api/books/{pageNumber}/{pageSize}/{orderBy}/{orderDirection}
        [HttpGet]
        [Route("{pageNumber:min(0)}/{pageSize:min(1)}")]
        public async Task<IHttpActionResult> GetBooks(int pageNumber, int pageSize)
        {
            try
            {
                var books = await this.searchClient.GetBooksAsync(pageNumber, pageSize);
                return Ok(new Result<JToken>(books, books != null ? ResultCode.Ok : ResultCode.Failed));
            }
            catch (Exception ex)
            {
                return Ok(new Result(ex.ToString()));
            }
        }

        // POST: api/books
        [HttpPost]
        [Route("")]
        [RequiredClaims(ActivityClaim.CreateBookClaim | ActivityClaim.EditBookClaim)]
        public async Task<IHttpActionResult> CreateOrUpdateBook(JObject rawBook)
        {
            try
            {
                var book = new DatabaseJObject(rawBook);
                if (book.ObjectTypeIsNot(DatabaseObjectType.BOOK, DatabaseObjectType.NONE))
                {
                    return Ok(new Result(DatabaseObjectType.BOOK, book.ObjectType));
                }
                else
                {
                    book.ObjectType = DatabaseObjectType.BOOK;
                    book = await this.databaseClient.CreateOrUpdateBookAsync(book);
                    if (book.ObjectId.HasValue)
                    {
                        await this.searchClient.CreateOrUpdateBookIndexAsync(book);
                        return Ok(new Result<ObjectIdResult>(new ObjectIdResult { ObjectId = book.ObjectId.Value }));
                    }
                    else
                    {
                        return Ok(new Result("No object ID could be found or created for the book."));
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new Result(ex.ToString()));
            }
        }

        // DELETE: api/books/{id}
        [HttpDelete]
        [Route("{id:int}")]
        [RequiredClaims(ActivityClaim.DeleteBookClaim)]
        public async Task<IHttpActionResult> DeleteBook(int id)
        {
            try
            {
                var result = await this.databaseClient.DeleteBookAsync(id);
                if (result)
                {
                    result &= await this.searchClient.DeleteBookIndexAsync(id);
                }
                return Ok(new Result(result ? ResultCode.Ok : ResultCode.Failed));
            }
            catch (Exception ex)
            {
                return Ok(new Result(ex.ToString()));
            }
        }
    }
}
