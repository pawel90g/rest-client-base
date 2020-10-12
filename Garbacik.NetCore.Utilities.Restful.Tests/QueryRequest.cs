using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Garbacik.NetCore.Utilities.Restful.Tests
{
    internal class QueryRequest
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
