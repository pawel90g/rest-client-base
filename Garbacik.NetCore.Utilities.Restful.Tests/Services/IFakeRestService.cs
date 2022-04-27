using Garbacik.NetCore.Utilities.Restful.Attributes.HttpRequest;
using RestSharp;

namespace Garbacik.NetCore.Utilities.Restful.Tests.Services;

internal interface IFakeRestService
{
    [HttpGet("fake-path")]
    RestRequest QueryParametersTest<T>(T request);

    [HttpPost("fake-path")]
    RestRequest FormRequestTest<T>(T request);
}