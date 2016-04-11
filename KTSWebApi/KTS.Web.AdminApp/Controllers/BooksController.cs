using KTS.Web.AdminApp.Attributes;
using KTS.Web.AdminApp.ViewModels;
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
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var booksData = await this.apiClient.GetBooks(null, 0, 1, this.Token);
            if (booksData.ResultCode == Enums.ResultCode.Ok)
            {
                return View(booksData.Data);
            }
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetBooks(int pageNumber, int pageSize)
        {
            var booksData = await this.apiClient.GetBooks(null, pageNumber, pageSize, this.Token);
            if (booksData.ResultCode == Enums.ResultCode.Ok)
            {
                var viewModel = new BooksNextRowsViewModel
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Books = booksData.Data
                };

                return PartialView("_BookRows", viewModel);
            }
            return PartialView("_BookRows");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit()
        {
            return RedirectToAction("Edit", new { id = 1 });
        }
    }
}