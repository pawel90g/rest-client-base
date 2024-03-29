﻿using RestSharp;

namespace Garbacik.NetCore.Utilities.Restful.Attributes.HttpRequest;

public sealed class HttpPostAttribute : HttpRequestBaseAttribute
{
    public HttpPostAttribute() : base(Method.Post) { }

    public HttpPostAttribute(string path) : base(Method.Post, path) { }
}