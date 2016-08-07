using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTS.Web.Enums
{
    public static class DatabaseFields
    {
        /** Common Database Fields **/
        public const string OBJECT_TYPE = "objectType";
        public const string OBJECT_ID = "objectID";

        /** User Object Fields **/
        public const string USERNAME = "username";
        public const string PASSWORD = "password";
        public const string USER_CLAIMS = "userClaims";

        /** Book Object Fields **/
        public const string AUTHOR = "author";
        public const string AUTHOR_FIRST = "author_first";
        public const string COVER_URL = "cover_url";
        public const string DESCRIPTION = "description";
        public const string GENRES = "genres";
        public const string PAGES = "pages";
        public const string PERMALINK = "permalink";
        public const string PRICE = "price";
        public const string PRINT_RUN = "print_run";
        public const string PUBLISHED_DATE = "published_date";
        public const string PUBLISHER = "publisher";
        public const string PUBLISHER_LOCATION = "publisher_location";
        public const string SEARCH_TITLE = "search_title";
        public const string SECTIONS = "sections";
        public const string TITLE = "title";
    }
}
