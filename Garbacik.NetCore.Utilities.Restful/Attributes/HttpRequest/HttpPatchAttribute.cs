using RestSharp;
namespace Garbacik.NetCore.Utilities.Restful.Attributes.HttpRequest;

public sealed class HttpPatchAttribute : HttpRequestBaseAttribute
{
    public HttpPatchAttribute() : base(Method.Patch) { }

    public HttpPatchAttribute(string path) : base(Method.Patch, path) { }
}