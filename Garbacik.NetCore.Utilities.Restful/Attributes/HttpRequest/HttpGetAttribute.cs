using RestSharp;

namespace Garbacik.NetCore.Utilities.Restful.Attributes.HttpRequest;

public sealed class HttpGetAttribute : HttpRequestBaseAttribute
{
    public HttpGetAttribute() : base() { }

    public HttpGetAttribute(string path) : base(Method.Get, path) { }
}