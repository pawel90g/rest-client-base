using RestSharp;

namespace Garbacik.NetCore.Utilities.Restful.Attributes.HttpRequest
{
    public sealed class HttpGetAttribute : HttpRequestBaseAttribute
    {
        public HttpGetAttribute() : base(Method.GET) { }

        public HttpGetAttribute(string path) : base(Method.GET, path) { }
    }
}
