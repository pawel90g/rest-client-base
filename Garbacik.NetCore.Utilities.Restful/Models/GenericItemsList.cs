using Newtonsoft.Json;

namespace Garbacik.NetCore.Utilities.Restful.Models
{
    public abstract class GenericItemsList<T>
    {
        [JsonProperty("items")]
        public T[] Items { get; set; }
    }
}
