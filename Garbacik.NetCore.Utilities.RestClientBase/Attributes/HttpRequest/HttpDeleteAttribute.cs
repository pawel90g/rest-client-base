using RestSharp;

namespace Garbacik.NetCore.Utilities.RestClientBase.Attributes.HttpRequest
{
    public sealed class HttpDeleteAttribute : HttpRequestBaseAttribute
    {
        public HttpDeleteAttribute() : base(Method.DELETE) { }

        public HttpDeleteAttribute(string path) : base(Method.DELETE, path) { }
    }
}
