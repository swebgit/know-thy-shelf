using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTS.Web.AdminApp.ViewModels
{
    public class AccountLoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
    }
}