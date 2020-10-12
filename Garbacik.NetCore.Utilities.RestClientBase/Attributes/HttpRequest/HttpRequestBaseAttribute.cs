using RestSharp;
using System;

namespace Garbacik.NetCore.Utilities.RestClientBase.Attributes.HttpRequest
{
    public abstract class HttpRequestBaseAttribute : Attribute
    {
        public Method Method { get; protected set; }
        public string Path { get; protected set; }

        public HttpRequestBaseAttribute(Method method = Method.GET, string path = "")
        {
            Method = method;
            Path = path;
        }
    }
}
