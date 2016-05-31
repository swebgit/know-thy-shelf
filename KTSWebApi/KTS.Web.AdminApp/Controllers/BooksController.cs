using KTS.Web.AdminApp.Attributes;
using KTS.Web.AdminApp.ViewModels;
using KTS.Web.Attributes;
using KTS.Web.Enums;
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
            var booksData = await this.apiClient.GetBooks(null, 0, 10, this.Token);
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
        public async Task<ActionResult> Edit(int id, string message = null)
        {
            var bookData = await this.apiClient.GetBook(id);
            if (bookData.ResultCode == Enums.ResultCode.Ok)
            {
                var viewModel = new BooksEditViewModel(bookData.Data);
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BooksEditViewModel viewModel)
        {
            return RedirectToAction("Edit", new { id = viewModel.ObjectId, message = "Save Successful" });
        }

        [HttpPost]
        public ActionResult AddSection(BooksEditViewModel viewModel)
        {
            if (viewModel != null)
            {
                int maxDisplayOrder;
                if (viewModel.Sections == null)
                {
                    viewModel.Sections = new List<BookEditSectionViewModel>();
                    maxDisplayOrder = 0;
                }
                else
                {
                    maxDisplayOrder = viewModel.Sections.Max(s => s.DisplayOrder);
                }

                viewModel.Sections.ForEach(s => s.Content = HttpUtility.HtmlDecode(s.Content));
                viewModel.Sections.Add(new BookEditSectionViewModel() { DisplayOrder = maxDisplayOrder + 1 });
                return PartialView ("EditorTemplates/BookSectionEditor", viewModel);
            }
            return this.HttpNotFound();
        }
    }
}