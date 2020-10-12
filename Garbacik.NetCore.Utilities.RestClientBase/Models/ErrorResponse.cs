using Newtonsoft.Json;

namespace Garbacik.NetCore.Utilities.RestClientBase.Models
{
    public class ErrorResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
