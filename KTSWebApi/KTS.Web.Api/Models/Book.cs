using Newtonsoft.Json;

namespace KTS.Web.Api.Models
{
    public class Book
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "originalTitle", NullValueHandling = NullValueHandling.Ignore)]
        public string OriginalTitle { get; set; }

        [JsonProperty(PropertyName = "commonTitle", NullValueHandling = NullValueHandling.Ignore)]
        public string CommonTitle { get; set; }

        [JsonProperty(PropertyName = "author", NullValueHandling = NullValueHandling.Ignore)]
        public string Author { get; set; }

        [JsonProperty(PropertyName = "publisher", NullValueHandling = NullValueHandling.Ignore)]
        public string Publisher { get; set; }

        [JsonProperty(PropertyName = "city", NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }

        [JsonProperty(PropertyName = "country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "publishMonthDay", NullValueHandling = NullValueHandling.Ignore)]
        public string PublishMonthDay { get; set; }

        [JsonProperty(PropertyName = "publishYear", NullValueHandling = NullValueHandling.Ignore)]
        public string PublishYear { get; set; }

        [JsonProperty(PropertyName = "printRun", NullValueHandling = NullValueHandling.Ignore)]
        public int? PrintRun { get; set; }

        [JsonProperty(PropertyName = "price", NullValueHandling = NullValueHandling.Ignore)]
        public string Price { get; set; }

        [JsonProperty(PropertyName = "pages", NullValueHandling = NullValueHandling.Ignore)]
        public int? Pages { get; set; }

        [JsonProperty(PropertyName = "series", NullValueHandling = NullValueHandling.Ignore)]
        public string Series { get; set; }
    }
}