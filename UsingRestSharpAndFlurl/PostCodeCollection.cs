using Newtonsoft.Json;
using System.Collections.Generic;

namespace UsingRestSharpAndFlurl
{
    public class PostCodeCollection
    {
        [JsonProperty(PropertyName = "postcodes")]
        public List<string> Postcodes { get; set; }
    }
}
