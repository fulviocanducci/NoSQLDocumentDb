using System;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace NoSQL.Library
{
    public class People
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        public static explicit operator People(Document v)
        {
            throw new NotImplementedException();
        }
    }
}
