using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Garbacik.NetCore.Utilities.RestClientBase.Models
{
    public class GenericResultItemsList<T>
    {
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("results")]
        public T[] Results { get; set; }
    }
}
