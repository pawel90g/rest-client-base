using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace Garbacik.NetCore.Utilities.Restful.Authenticators;

public class AuthorizationHeaderAuthenticator : IAuthenticator
{
    private readonly string authorizationHeaderValue;
        
    public AuthorizationHeaderAuthenticator(string authorizationHeaderValue)
    {
        this.authorizationHeaderValue = authorizationHeaderValue;
    }

    public ValueTask Authenticate(RestClient client, RestRequest request)
    {
        request.AddHeader("Authorization", authorizationHeaderValue);
        return ValueTask.CompletedTask;
    }
}