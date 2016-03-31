using KTS.Web.AdminApp.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KTS.Web.AdminApp.Controllers
{
    [JwtOptionalAuthorize]
    public class HomeController : BaseController
    {
        public ActionResult Index(bool loggedOut = false)
        {
            return View();
        }
    }
}