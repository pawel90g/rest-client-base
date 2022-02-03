using Garbacik.NetCore.Utilities.Restful.Consts;
using Garbacik.NetCore.Utilities.Restful.Models;
using Garbacik.NetCore.Utilities.Restful.Tests.Services;
using RestSharp;
using Xunit;

namespace Garbacik.NetCore.Utilities.Restful.Tests;

public class UnitTest1
{
    private readonly IFakeRestService fakeRestService;

    QueryRequest queryRequest = new QueryRequest
    {
        Id = 1,
        Name = "fake-name"
    };

    public UnitTest1()
    {
        fakeRestService = new FakeRestService();
    }

    [Fact]
    public void QueryParameteresTest()
    {
        var request = fakeRestService.QueryParametersTest(queryRequest);
        Assert.Equal(2, request.Parameters.Count);

        foreach (var p in request.Parameters)
            Assert.Equal(ParameterType.QueryString, p.Type);

        Assert.Contains(request.Parameters, x => x.Name == "id");
        Assert.Contains(request.Parameters, x => x.Name == "name");

        Assert.DoesNotContain(request.Parameters, x => x.Name == "Id");
        Assert.DoesNotContain(request.Parameters, x => x.Name == "Name");
    }

    [Fact]
    public void TestXmlResponse()
    {
        var response = new GenericResponse<Response>(new RestResponse
        {
            Content = System.IO.File.ReadAllText("response_unit_test.xml"),
            ContentType = ContentTypes.XML,
            StatusCode = System.Net.HttpStatusCode.OK,
            ResponseStatus = ResponseStatus.Completed,
            IsSuccessful = true
        });

        Assert.Equal(2020, response.Response.Id);
        Assert.Equal(2, response.Response.Items.Count);
        Assert.Equal(2, response.Response.Values.Count);
    }
}