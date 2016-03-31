using KTS.Web.AdminApp.ViewModels;
using KTS.Web.Enums;
using KTS.Web.Interfaces;
using KTS.Web.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KTS.Web.AdminApp.Controllers
{
    public class AccountController : BaseController
    {
        private IKTSApiClient apiClient { get; set; }

        public AccountController(IKTSApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public ActionResult Login(string username, string errorMessage = "")
        {
            var viewModel = new AccountLoginViewModel
            {
                Username = username,
                ErrorMessage = errorMessage
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(AccountLoginViewModel viewModel)
        {
            var loginResult = await this.apiClient.LogOn(new Credentials { Username = viewModel.Username, Password = viewModel.Password });
            if (loginResult.ResultCode == ResultCode.Ok)
            {
                // Save Auth token to a cookie
                this.ControllerContext.HttpContext.Response.Cookies.Remove("KTS-AuthToken");
                this.ControllerContext.HttpContext.Response.Cookies.Add(new HttpCookie("KTS-AuthToken", loginResult.Data.Token));
                return RedirectToAction("Index", "Home");
            }
            else
            {
                viewModel.ErrorMessage = "Login failed.";
                viewModel.Password = String.Empty;
                return RedirectToAction("Login", "Account", viewModel);
            }
        }
    }
}