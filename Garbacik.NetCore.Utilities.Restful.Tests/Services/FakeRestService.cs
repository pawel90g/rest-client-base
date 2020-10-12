using RestSharp;

namespace Garbacik.NetCore.Utilities.Restful.Tests.Services
{
    internal class FakeRestService : RestClientBase, IFakeRestService
    {
        public FakeRestService() : base(new RestClient())
        {
        }

        public IRestRequest QueryParameteresTest(QueryRequest request)
        {
            var (method, path) = GetDataFromAttributes();
            return CreateQueryRequest(path, method, DataFormat.Json, request);
        }
    }
}
