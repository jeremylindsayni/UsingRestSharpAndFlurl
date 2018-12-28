using System.Collections.Generic;

namespace UsingRestSharpAndFlurl
{
    public class GeocodeResponseCollection
    {
        public string Status { get; set; }

        public List<QueryResult> Result { get; set; }
    }
}