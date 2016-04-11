using KTS.Web.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTS.Web.AdminApp.ViewModels
{
    public class BooksNextRowsViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<Book> Books { get; set; }
    }
}