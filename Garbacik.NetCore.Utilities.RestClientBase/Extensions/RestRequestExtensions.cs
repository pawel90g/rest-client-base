using RestSharp;
using System.Collections.Generic;

namespace Garbacik.NetCore.Utilities.RestClientBase.Extensions
{
    public static class RestRequestExtensions
    {
        public static void PrepareHeaders(this IRestRequest restRequest, IDictionary<string, string> headers)
        {
            foreach (KeyValuePair<string, string> header in headers)
            {
                restRequest.Parameters.RemoveAll((param) => param.Name.Equals(header.Key) && param.Type == ParameterType.HttpHeader);
                restRequest.AddHeader(header.Key, header.Value);
            }
        }
    }
}
