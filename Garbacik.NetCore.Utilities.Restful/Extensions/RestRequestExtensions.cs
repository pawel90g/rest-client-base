using RestSharp;
using System.Collections.Generic;
using System.Linq;

namespace Garbacik.NetCore.Utilities.Restful.Extensions;

public static class RestRequestExtensions
{
    public static void PrepareHeaders(this RestRequest restRequest, IDictionary<string, string> headers)
    {
        foreach (var (key, value) in headers)
        {
            var parameter = restRequest.Parameters
                .FirstOrDefault(param => param.Name != null
                                         && param.Name.Equals(key)
                                         && param.Type == ParameterType.HttpHeader);
                
            if (parameter != null)
                restRequest.Parameters.RemoveParameter(parameter);

            restRequest.AddHeader(key, value);
        }
    }
}