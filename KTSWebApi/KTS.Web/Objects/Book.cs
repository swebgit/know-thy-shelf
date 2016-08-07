using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTS.Web.Objects
{
    public class Book
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("author")]
        public string Author { get; set; }
        [JsonProperty("author_first")]
        public string AuthorFirstName { get; set; }
        [JsonProperty("objectID")]
        public int ObjectId { get; set; }
        public string AuthorName
        {
            get
            {
                return $"{this.AuthorFirstName} {this.Author}";
            }
        }
    }
}
