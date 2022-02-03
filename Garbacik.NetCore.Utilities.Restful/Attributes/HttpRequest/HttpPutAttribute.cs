using RestSharp;
namespace Garbacik.NetCore.Utilities.Restful.Attributes.HttpRequest;

public sealed class HttpPutAttribute : HttpRequestBaseAttribute
{
    public HttpPutAttribute() : base(Method.Put) { }
    public HttpPutAttribute(string path) : base(Method.Put, path) { }
}