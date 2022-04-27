using Garbacik.NetCore.Utilities.Restful.Consts;
using Garbacik.NetCore.Utilities.Restful.Models;
using Garbacik.NetCore.Utilities.Restful.Tests.Services;
using RestSharp;
using Xunit;

namespace Garbacik.NetCore.Utilities.Restful.Tests;

public class RestClientBaseMethodsTests
{
    private readonly IFakeRestService fakeRestService;

    private readonly JsonPropertiesClass jsonPropertiesClassInstance = new()
    {
        PropertyInt = 1,
        PropertyString = "fake-name"
    };
    
    private readonly JsonObjectAnnotationClass jsonObjectAnnotationClassInstance = new()
    {
        PropertyInt = 1,
        PropertyString = "test"
    };

    public RestClientBaseMethodsTests()
    {
        fakeRestService = new FakeRestService();
    }

    [Fact]
    public void QueryParametersTest()
    {
        var request = fakeRestService.QueryParametersTest(jsonPropertiesClassInstance);
        Assert.Equal(2, request.Parameters.Count);

        foreach (var p in request.Parameters)
            Assert.Equal(ParameterType.QueryString, p.Type);

        Assert.Contains(request.Parameters, x => x.Name == "propertyInt");
        Assert.Contains(request.Parameters, x => x.Name == "propertyString");

        Assert.DoesNotContain(request.Parameters, x => x.Name == "PropertyInt");
        Assert.DoesNotContain(request.Parameters, x => x.Name == "PropertyString");
    }

    [Fact]
    public void FormRequestTest()
    {
        var request = fakeRestService.FormRequestTest(jsonObjectAnnotationClassInstance);
        Assert.Equal(2, request.Parameters.Count);

        foreach (var p in request.Parameters)
            Assert.Equal(ParameterType.RequestBody, p.Type);

        Assert.Contains(request.Parameters, x => x.Name == "propertyInt");
        Assert.Contains(request.Parameters, x => x.Name == "propertyString");

        Assert.DoesNotContain(request.Parameters, x => x.Name == "PropertyInt");
        Assert.DoesNotContain(request.Parameters, x => x.Name == "PropertyString");
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