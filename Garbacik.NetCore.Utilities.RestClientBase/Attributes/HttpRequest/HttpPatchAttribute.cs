﻿using RestSharp;
namespace Garbacik.NetCore.Utilities.RestClientBase.Attributes.HttpRequest
{
    public sealed class HttpPatchAttribute : HttpRequestBaseAttribute
    {
        public HttpPatchAttribute() : base(Method.PATCH) { }

        public HttpPatchAttribute(string path) : base(Method.PATCH, path) { }
    }
}
