using KTS.Web.AdminApp.Attributes;
using KTS.Web.Attributes;
using KTS.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KTS.Web.AdminApp.Controllers
{
    [JwtAuthorize]
    public class BooksController : BaseController
    {
        private IKTSApiClient apiClient { get; set; }

        public BooksController(IKTSApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        // GET: Books
        public async Task<ActionResult> Index()
        {
            var booksData = await this.apiClient.GetBooks(null, 0, 10, this.Token);
            return View();
        }
    }
}