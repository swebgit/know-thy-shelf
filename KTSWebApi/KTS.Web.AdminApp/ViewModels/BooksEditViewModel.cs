using KTS.Web.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KTS.Web.AdminApp.ViewModels
{
    public class BooksEditViewModel
    {
        public int? ObjectId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Display(Name = "Cover Image URL")]
        public string CoverUrl { get; set; }
        public string Publisher { get; set; }
        [Display(Name = "Publisher Location")]
        public string PublisherLocation { get; set; }
        [Display(Name = "Published Date")]
        public string PublishedDate { get; set; }
        public List<string> Genres { get; set; }
        public int? Pages { get; set; }
        public decimal? Price { get; set; }
        [Display(Name = "Print Run")]
        public int? PrintRun { get; set; }
        public string Description { get; set; }
        public string Permalink { get; set; }
        public List<BookEditSectionViewModel> Sections { get; set; }

        public BooksEditViewModel()
        {

        }

        public BooksEditViewModel(JObject jsonData)
        {
            this.ObjectId = jsonData.GetValue<int?>(DatabaseFields.OBJECT_ID);
            this.Title = jsonData.GetValue<string>(DatabaseFields.TITLE);
            this.Author = jsonData.GetValue<string>(DatabaseFields.AUTHOR);
            this.CoverUrl = jsonData.GetValue<string>(DatabaseFields.COVER_URL);
            this.Genres = jsonData.GetValue<List<string>>(DatabaseFields.GENRES);
            this.Sections = jsonData.GetValue<List<BookEditSectionViewModel>>(DatabaseFields.SECTIONS);
        }

    }
    
    public class BookEditSectionViewModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int DisplayOrder { get; set; }
    }
}