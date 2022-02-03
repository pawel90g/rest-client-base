using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace Garbacik.NetCore.Utilities.Restful.Authenticators;

public class BearerAuthenticator : IAuthenticator
{
    private readonly string token;

    public BearerAuthenticator(string token)
    {
        this.token = token;
    }

    public ValueTask Authenticate(RestClient client, RestRequest request)
    {
        request.AddHeader("Authorization", $"Bearer {token}");
        return ValueTask.CompletedTask;
    }
}