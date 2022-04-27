using RestSharp;

namespace Garbacik.NetCore.Utilities.Restful.Tests.Services;

internal class FakeRestService : RestClientBase, IFakeRestService
{
    public FakeRestService() : base("http://fake-url")
    {
    }

    public RestRequest QueryParametersTest<T>(T request)
    {
        var (method, path) = GetDataFromAttributes();
        return CreateQueryRequest(path, method, request);
    }

    public RestRequest FormRequestTest<T>(T request)
    {
        var (method, path) = GetDataFromAttributes();
        return CreateFormRequest(path, method, request);
    }
}