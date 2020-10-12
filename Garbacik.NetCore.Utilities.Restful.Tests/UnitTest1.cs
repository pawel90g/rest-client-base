using Garbacik.NetCore.Utilities.Restful.Tests.Services;
using RestSharp;
using Xunit;

namespace Garbacik.NetCore.Utilities.Restful.Tests
{
    public class UnitTest1
    {
        private readonly IFakeRestService fakeRestService;
        public UnitTest1()
        {
            fakeRestService = new FakeRestService();
        }

        [Fact]
        public void QueryParameteresTest()
        {
            var queryRequest = new QueryRequest
            {
                Id = 1,
                Name = "fake-name"
            };

            var request = fakeRestService.QueryParameteresTest(queryRequest);
            Assert.Equal(2, request.Parameters.Count);

            request.Parameters.ForEach(p =>
            {
                Assert.Equal(ParameterType.QueryString, p.Type);
            });

            Assert.Contains(request.Parameters, x => x.Name == "id");
            Assert.Contains(request.Parameters, x => x.Name == "name");

            Assert.DoesNotContain(request.Parameters, x => x.Name == "Id");
            Assert.DoesNotContain(request.Parameters, x => x.Name == "Name");
        }
    }
}
