using KTS.Web.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            this.Genres = new List<string>();
            this.Sections = new List<BookEditSectionViewModel>();
        }

        public BooksEditViewModel(JObject jsonData)
        {
            this.ObjectId = jsonData.GetValue<int?>(DatabaseFields.OBJECT_ID);
            this.Title = jsonData.GetValue<string>(DatabaseFields.TITLE);
            this.Author = jsonData.GetValue<string>(DatabaseFields.AUTHOR);
            this.CoverUrl = jsonData.GetValue<string>(DatabaseFields.COVER_URL);
            this.Publisher = jsonData.GetValue<string>(DatabaseFields.PUBLISHER);
            this.PublisherLocation = jsonData.GetValue<string>(DatabaseFields.PUBLISHER_LOCATION);
            this.PublishedDate = jsonData.GetValue<string>(DatabaseFields.PUBLISHED_DATE);
            this.Genres = jsonData.GetValue<List<string>>(DatabaseFields.GENRES);
            this.Pages = jsonData.GetValue<int?>(DatabaseFields.PAGES);
            this.Price = jsonData.GetValue<decimal?>(DatabaseFields.PRICE);
            this.PrintRun = jsonData.GetValue<int?>(DatabaseFields.PRINT_RUN);
            this.Description = jsonData.GetValue<string>(DatabaseFields.DESCRIPTION);
            this.Permalink = jsonData.GetValue<string>(DatabaseFields.PERMALINK);
            this.Sections = jsonData.GetValue<List<BookEditSectionViewModel>>(DatabaseFields.SECTIONS);
        }

        public JObject ToJObject()
        {
            var jObject = new JObject();

            if (this.ObjectId.HasValue)
                jObject.Add(DatabaseFields.OBJECT_ID, this.ObjectId);

            jObject.Add(DatabaseFields.TITLE, this.Title);
            jObject.Add(DatabaseFields.AUTHOR, this.Author);
            jObject.Add(DatabaseFields.COVER_URL, this.CoverUrl);
            jObject.Add(DatabaseFields.PUBLISHER, this.Publisher);
            jObject.Add(DatabaseFields.PUBLISHER_LOCATION, this.PublisherLocation);
            jObject.Add(DatabaseFields.PUBLISHED_DATE, this.PublishedDate);
            jObject.Add(DatabaseFields.GENRES, JToken.FromObject(this.Genres));
            jObject.Add(DatabaseFields.PAGES, this.Pages);
            jObject.Add(DatabaseFields.PRICE, this.Price);
            jObject.Add(DatabaseFields.PRINT_RUN, this.PrintRun);
            jObject.Add(DatabaseFields.DESCRIPTION, this.Description);
            jObject.Add(DatabaseFields.PERMALINK, this.Permalink);
            jObject.Add(DatabaseFields.SECTIONS, JToken.FromObject(this.Sections));

            return jObject;
        }

    }
    
    public class BookEditSectionViewModel
    {
        public string Title { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public int DisplayOrder { get; set; }
    }
}