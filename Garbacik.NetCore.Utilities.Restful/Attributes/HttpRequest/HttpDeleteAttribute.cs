using RestSharp;

namespace Garbacik.NetCore.Utilities.Restful.Attributes.HttpRequest;

public sealed class HttpDeleteAttribute : HttpRequestBaseAttribute
{
    public HttpDeleteAttribute() : base(Method.Delete) { }

    public HttpDeleteAttribute(string path) : base(Method.Delete, path) { }
}