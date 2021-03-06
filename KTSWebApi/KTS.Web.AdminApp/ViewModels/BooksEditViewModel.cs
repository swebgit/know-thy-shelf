﻿using KTS.Web.Enums;
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
        [Display(Name = "Search Title")]
        public string SearchTitle { get; set; }
        [Required]
        [Display(Name = "Author Last Name")]
        public string Author { get; set; }
        [Required]
        [Display(Name = "Author First Name")]
        public string AuthorFirstName { get; set; }
        [Display(Name = "Cover Image URL")]
        public string CoverUrl { get; set; }
        public string Publisher { get; set; }
        [Display(Name = "Publisher City")]
        public string PublisherLocation { get; set; }
        [Display(Name = "Publisher Country")]
        public string PublisherLocationCountry { get; set; }
        [Display(Name = "Published Date")]
        public string PublishedDate { get; set; }
        public List<string> Genres { get; set; }
        public string Pages { get; set; }
        public string Price { get; set; }
        [Display(Name = "Print Run")]
        public string PrintRun { get; set; }
        [AllowHtml]
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
            this.SearchTitle = jsonData.GetValue<string>(DatabaseFields.SEARCH_TITLE);
            this.Author = jsonData.GetValue<string>(DatabaseFields.AUTHOR);
            this.AuthorFirstName = jsonData.GetValue<string>(DatabaseFields.AUTHOR_FIRST);
            this.CoverUrl = jsonData.GetValue<string>(DatabaseFields.COVER_URL);
            this.Publisher = jsonData.GetValue<string>(DatabaseFields.PUBLISHER);
            this.PublisherLocation = jsonData.GetValue<string>(DatabaseFields.PUBLISHER_LOCATION);
            this.PublisherLocationCountry = jsonData.GetValue<string>(DatabaseFields.PUBLISHER_LOCATION_COUNTRY);
            this.PublishedDate = jsonData.GetValue<string>(DatabaseFields.PUBLISHED_DATE);
            this.Genres = jsonData.GetValue<List<string>>(DatabaseFields.GENRES);
            this.Pages = jsonData.GetValue<string>(DatabaseFields.PAGES);
            this.Price = jsonData.GetValue<string>(DatabaseFields.PRICE);
            this.PrintRun = jsonData.GetValue<string>(DatabaseFields.PRINT_RUN);
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
            jObject.Add(DatabaseFields.SEARCH_TITLE, this.SearchTitle);
            jObject.Add(DatabaseFields.AUTHOR, this.Author);
            jObject.Add(DatabaseFields.AUTHOR_FIRST, this.AuthorFirstName);
            jObject.Add(DatabaseFields.COVER_URL, this.CoverUrl);
            jObject.Add(DatabaseFields.PUBLISHER, this.Publisher);
            jObject.Add(DatabaseFields.PUBLISHER_LOCATION, this.PublisherLocation);
            jObject.Add(DatabaseFields.PUBLISHER_LOCATION_COUNTRY, this.PublisherLocationCountry);
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