using Garbacik.NetCore.Utilities.Restful.Attributes;
using Garbacik.NetCore.Utilities.Restful.Attributes.HttpRequest;
using Garbacik.NetCore.Utilities.Restful.Attributes.RequestParamTypes;
using Garbacik.NetCore.Utilities.Restful.Authenticators;
using Garbacik.NetCore.Utilities.Restful.Extensions;
using Garbacik.NetCore.Utilities.Restful.Models;
using Garbacik.NetCore.Utilities.Restful.Tools;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Garbacik.NetCore.Utilities.Restful
{
    public abstract class RestClientBase
    {
        protected readonly IRestClient _restClient;

        private RestClientBase() { }

        protected RestClientBase(IRestClient restClient)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };
            _restClient = restClient;
        }

        protected (Method Method, string Path) GetDataFromAttributes()
        {
            try
            {
                var (@interface, methodDefinition) = FindProperInterface(new StackTrace());
                var methodName = methodDefinition.Name;

                var method = @interface.GetMethod(methodName);
                var httpRequestAttr = @interface.GetMethod(methodName).GetCustomAttribute<HttpRequestBaseAttribute>(true);
                var pathProperties = @interface.GetPropertiesWithAttribute<PathParamAttribute>();

                var attr = methodDefinition.GetCustomAttribute<HttpRequestBaseAttribute>(true);
                if (attr != null)
                    httpRequestAttr = attr;

                if (httpRequestAttr == null)
                    return (Method.GET, "/");

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
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected IRestRequest CreateRequest(string resource, Method method)
        {
            return CreateRequest(resource, method, DataFormat.Json);
        }

        protected IRestRequest CreateRequest(string resource, Method method, DataFormat dataFormat)
        {
            return new RestRequest(resource, method, dataFormat)
            {
                JsonSerializer = NewtonsoftJsonSerializer.Default
            };
        }

        protected IRestRequest CreateQueryRequest<T>(string resource, Method method, DataFormat dataFormat, T queryObject)
        {
            var request = CreateRequest(resource, method, dataFormat);

            foreach (var prop in queryObject.GetType().GetProperties())
            {
                var propertyName = prop.GetCustomAttribute<JsonPropertyAttribute>(true)?.PropertyName ?? prop.Name;
                var propertyValue = prop.GetValue(queryObject);

                if (propertyValue == null)
                    continue;

                request.AddQueryParameter(propertyName, propertyValue.ToString(), true);
            }

            return request;
        }

        protected IRestRequest CreateFormRequest<T>(string resource, Method method, DataFormat dataFormat, T requestObject)
        {
            var request = CreateRequest(resource, method, dataFormat);

            foreach (var prop in requestObject.GetType().GetProperties())
            {
                var propertyName = prop.GetCustomAttribute<JsonPropertyAttribute>(true).PropertyName;
                var propertyValue = prop.GetValue(requestObject);

                if (propertyValue == null)
                    continue;

                var isFile = prop.GetCustomAttributes(typeof(FilePropertyAttribute), true).Any();

                if (isFile)
                {
                    if (propertyValue is byte[] fileBytes)
                    {
                        request.AddFileBytes(propertyName, fileBytes, DateTime.Now.ToString("yyyyMMddHHmmss"));
                    }
                    else if (propertyValue is string filePath)
                    {
                        request.AddFile(propertyName, filePath);
                    }
                    else if (propertyValue is FileDescription fileDescription)
                    {
                        request.AddFile(propertyName, fileDescription.FileBytes, fileDescription.FileName);
                    }
                }
                else
                {
                    request.AddParameter(propertyName, propertyValue);
                }
            }

            return request;
        }

        protected IRestRequest CreateBodyRequest<T>(string resource, Method method, T requestObject)
        {
            return CreateBodyRequest<T>(resource, method, DataFormat.Json, requestObject);
        }

        protected IRestRequest CreateMultiPartBodyRequest<T>(string resource, Method method, T requestObject)
        {
            var request = CreateRequest(resource, method, DataFormat.None);
            request.AddObject(requestObject);
            request.AlwaysMultipartFormData = true;
            return request;
        }

        protected IRestRequest CreateBodyRequest<T>(string resource, Method method, DataFormat dataFormat, T requestObject)
        {
            var request = CreateRequest(resource, method, dataFormat);

            if (dataFormat == DataFormat.Xml)
            {
                request.AddXmlBody(requestObject);
            }
            else
            {
                request.AddJsonBody(requestObject);
            }

            return request;
        }

        protected async Task<GenericResponse> ExecuteTaskForResponseAsync(IRestRequest request)
        {
            var response = await HandleUnauthorizedAsync(
                await _restClient.ExecuteAsync(request));
            return new GenericResponse(response);
        }

        protected async Task<GenericResponse<T>> ExecuteTaskForResponseAsync<T>(IRestRequest request)
        {
            var response = await HandleUnauthorizedAsync(
                    await _restClient.ExecuteAsync(request)
                );
            return new GenericResponse<T>(response);
        }

        private async Task<IRestResponse> HandleUnauthorizedAsync(IRestResponse restResponse)
        {
            if (restResponse.StatusCode == HttpStatusCode.Unauthorized
                && _restClient.Authenticator is IUnauthorizedClient xsrfAuthenticator)
            {
                xsrfAuthenticator.MarkAsUnauthorized();
                return await _restClient.ExecuteAsync(restResponse.Request);
            }

            return restResponse;
        }

        private (Type @interface, MethodBase methodDefinition) FindProperInterface(StackTrace stackTrace, int frameIndex = 0)
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
}
