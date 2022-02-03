using RestSharp;

namespace Garbacik.NetCore.Utilities.Restful.Tests.Services;

internal class FakeRestService : RestClientBase, IFakeRestService
{
    public FakeRestService() : base(new RestClient())
    {
    }

    public RestRequest QueryParametersTest(QueryRequest request)
    {
        var (method, path) = GetDataFromAttributes();
        return CreateQueryRequest(path, method, request);
    }
}