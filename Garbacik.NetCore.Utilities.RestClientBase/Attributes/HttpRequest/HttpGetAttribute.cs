using RestSharp;

namespace Garbacik.NetCore.Utilities.RestClientBase.Attributes.HttpRequest
{
    public sealed class HttpGetAttribute : HttpRequestBaseAttribute
    {
        public HttpGetAttribute() : base(Method.GET) { }

        public HttpGetAttribute(string path) : base(Method.GET, path) { }
    }
}
