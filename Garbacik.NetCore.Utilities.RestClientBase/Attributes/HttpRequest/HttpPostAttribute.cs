using RestSharp;

namespace Garbacik.NetCore.Utilities.RestClientBase.Attributes.HttpRequest
{
    public sealed class HttpPostAttribute : HttpRequestBaseAttribute
    {
        public HttpPostAttribute() : base(Method.POST) { }

        public HttpPostAttribute(string path) : base(Method.POST, path) { }
    }
}
