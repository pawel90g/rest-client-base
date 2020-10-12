using RestSharp;
namespace Garbacik.NetCore.Utilities.Restful.Attributes.HttpRequest
{
    public sealed class HttpPutAttribute : HttpRequestBaseAttribute
    {
        public HttpPutAttribute() : base(Method.PUT) { }
        public HttpPutAttribute(string path) : base(Method.PUT, path) { }
    }
}
