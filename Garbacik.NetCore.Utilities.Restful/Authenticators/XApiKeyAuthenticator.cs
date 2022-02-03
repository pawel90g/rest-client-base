using Garbacik.NetCore.Utilities.Restful.Extensions;
using RestSharp;
using RestSharp.Authenticators;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Garbacik.NetCore.Utilities.Restful.Authenticators;

public class XApiKeyAuthenticator : IAuthenticator
{
    private readonly string xApiKey;

    public XApiKeyAuthenticator(string xApiKey)
    {
        this.xApiKey = xApiKey;
    }

    public ValueTask Authenticate(RestClient client, RestRequest request)
    {
        request.PrepareHeaders(new Dictionary<string, string>
        {
            { "X-API-Key", xApiKey }
        });
        
        return ValueTask.CompletedTask;
    }
}