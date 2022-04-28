using Garbacik.NetCore.Utilities.Restful.Attributes;
using Garbacik.NetCore.Utilities.Restful.Attributes.HttpRequest;
using Garbacik.NetCore.Utilities.Restful.Attributes.RequestParamTypes;
using Garbacik.NetCore.Utilities.Restful.Authenticators;
using Garbacik.NetCore.Utilities.Restful.Extensions;
using Garbacik.NetCore.Utilities.Restful.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using RestSharp.Authenticators;
using RestSharp.Serializers.NewtonsoftJson;

namespace Garbacik.NetCore.Utilities.Restful;

public abstract class RestClientBase
{
    private readonly RestClient restClient;

    private RestClientBase()
    {
    }

    protected RestClientBase(string url) =>
        restClient = new RestClient(url);

    protected RestClientBase(string url, bool skipSslVerification) =>
        restClient = skipSslVerification
            ? new RestClient(
                new RestClientOptions
                {
                    BaseUrl = new Uri(url ?? throw new ArgumentNullException(nameof(url))),
                    RemoteCertificateValidationCallback = (_, _, _, _) => true,
                })
            : new RestClient(url);

    protected RestClientBase(string url, IAuthenticator authenticator) =>
        restClient = new RestClient(url)
        {
            Authenticator = authenticator
        };

    protected RestClientBase(string url, IAuthenticator authenticator, bool skipSslVerification) =>
        restClient = skipSslVerification
            ? new RestClient(
                new RestClientOptions
                {
                    BaseUrl = new Uri(url ?? throw new ArgumentNullException(nameof(url))),
                    RemoteCertificateValidationCallback = (_, _, _, _) => true,
                })
            {
                Authenticator = authenticator
            }
            : new RestClient(url)
            {
                Authenticator = authenticator
            };

    protected void UseNewtonsoftJsonSerializer() => restClient.UseNewtonsoftJson();

    protected (Method Method, string Path) GetDataFromAttributes()
    {
        try
        {
            var (@interface, methodDefinition) = FindProperInterface(new StackTrace());
            var methodName = methodDefinition.Name;

            var httpRequestAttr = @interface.GetMethod(methodName).GetCustomAttribute<HttpRequestBaseAttribute>(true);
            var pathProperties = @interface.GetPropertiesWithAttribute<PathParamAttribute>();

            var attr = methodDefinition.GetCustomAttribute<HttpRequestBaseAttribute>(true);
            if (attr != null)
                httpRequestAttr = attr;

            if (httpRequestAttr == null)
                return (Method.Get, "/");

            var pathProps = methodDefinition.ReflectedType.GetPropertiesWithAttribute<PathParamAttribute>();
            if (pathProps != null && pathProps.Any())
            {
                pathProperties = pathProps;
            }

            var path = httpRequestAttr.Path;
            foreach (var prop in pathProperties)
            {
                var val = GetType().GetProperty(prop.Name).GetValue(this);
                if (val == null)
                    continue;

                var pathAttr = prop.GetCustomAttribute<PathParamAttribute>(true);
                var name = pathAttr?.ParamName ?? prop.Name;
                ;
                path = path.Replace($"{{{name}}}", val.ToString());
            }

            return (httpRequestAttr.Method, path);
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected static RestRequest CreateSimpleRequest(string resource, Method method)
        => new RestRequest(resource, method);

    protected static RestRequest CreateQueryRequest<T>(string resource, Method method, T queryObject)
    {
        var request = new RestRequest(resource, method);

        foreach (var prop in queryObject.GetType().GetProperties())
        {
            var propertyName = prop.GetCustomAttribute<JsonPropertyAttribute>(true)?.PropertyName ?? prop.Name;
            var propertyValue = prop.GetValue(queryObject);

            if (propertyValue is null)
                continue;

            request.AddQueryParameter(propertyName, propertyValue.ToString(), true);
        }

        return request;
    }

    protected static RestRequest CreateFormRequest<T>(string resource, Method method, T requestObject)
    {
        var request = new RestRequest(resource, method);

        var jsonObjectNamingStrategyType = requestObject
            .GetType()
            .GetCustomAttribute<JsonObjectAttribute>(true)?
            .NamingStrategyType;

        foreach (var prop in requestObject.GetType().GetProperties())
        {
            var propertyName = prop.Name;
            if (jsonObjectNamingStrategyType is not null)
            {
                if (jsonObjectNamingStrategyType == typeof(CamelCaseNamingStrategy))
                    propertyName = prop.Name.ToCamelCase();
            }
            else
            {
                var jsonPropertyAttribute = prop.GetCustomAttribute<JsonPropertyAttribute>(true);
                if (jsonPropertyAttribute is not null)
                    propertyName = jsonPropertyAttribute.PropertyName;
            }

            var propertyValue = prop.GetValue(requestObject);

            if (propertyValue is null || propertyName is null)
                continue;

            var isFile = prop.GetCustomAttributes(typeof(FilePropertyAttribute), true).Any();

            if (isFile)
            {
                switch (propertyValue)
                {
                    case byte[] fileBytes:
                        request.AddFile(propertyName, fileBytes, DateTime.Now.ToString("yyyyMMddHHmmss"));
                        break;
                    case string filePath:
                        request.AddFile(propertyName, filePath);
                        break;
                    case FileDescription fileDescription:
                        request.AddFile(propertyName, fileDescription.FileBytes, fileDescription.FileName);
                        break;
                }
            }
            else
            {
                request.AddParameter(propertyName, propertyValue, ParameterType.RequestBody);
            }
        }

        return request;
    }

    protected static RestRequest CreateBodyRequest<T>(string resource, Method method, T requestObject)
        where T : class =>
        CreateBodyRequest(resource, method, DataFormat.Json, requestObject);

    protected static RestRequest CreateMultiPartBodyRequest<T>(string resource, Method method, T requestObject)
        where T : class
    {
        var request = new RestRequest(resource, method);
        request.AddObject(requestObject);
        request.AlwaysMultipartFormData = true;
        return request;
    }

    protected static RestRequest CreateBodyRequest<T>(string resource, Method method, DataFormat dataFormat,
        T requestObject) where T : class
    {
        var request = new RestRequest(resource, method);

        if (dataFormat == DataFormat.Xml)
            request.AddXmlBody(requestObject);
        else
            request.AddJsonBody(requestObject);

        return request;
    }

    protected async Task<GenericResponse> ExecuteTaskForResponseAsync(RestRequest request) =>
        new(await HandleUnauthorizedAsync(
            await restClient.ExecuteAsync(request)));

    protected async Task<GenericResponse<T>> ExecuteTaskForResponseAsync<T>(RestRequest request) =>
        new(await HandleUnauthorizedAsync(
            await restClient.ExecuteAsync(request)
        ));

    protected async Task<Stream> ExecuteStreamDownloadAsync(RestRequest request) =>
        await restClient.DownloadStreamAsync(request);

    protected async Task<byte[]> ExecuteDataDownloadAsync(RestRequest request) =>
        await restClient.DownloadDataAsync(request);

    private async Task<RestResponse> HandleUnauthorizedAsync(RestResponse restResponse)
    {
        if (restResponse.StatusCode != HttpStatusCode.Unauthorized
            || restClient.Authenticator is not IUnauthorizedClient xsrfAuthenticator)
            return restResponse;

        xsrfAuthenticator.MarkAsUnauthorized();
        return await restClient.ExecuteAsync(restResponse.Request);
    }

    private static (Type @interface, MethodBase methodDefinition) FindProperInterface(StackTrace stackTrace,
        int frameIndex = 0)
    {
        var methodDefinition = stackTrace.GetFrame(frameIndex).GetMethod();
        var methodName = methodDefinition.Name;
        var className = methodDefinition.ReflectedType.Name;

        Type @interface;

        if ((@interface = methodDefinition.DeclaringType.GetInterface($"I{className}")) != null)
        {
            if (@interface.GetMethod(methodName)
                    .GetCustomAttribute<HttpRequestBaseAttribute>(true) == null)
            {
                return FindProperInterface(stackTrace, ++frameIndex);
            }

            return (@interface, methodDefinition);
        }

        return FindProperInterface(stackTrace, ++frameIndex);
    }
}