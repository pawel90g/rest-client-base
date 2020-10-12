using RestSharp;
using RestSharp.Authenticators;

namespace Garbacik.NetCore.Utilities.RestClientBase.Authenticators
{
    public class BearerAthenticator : IAuthenticator
    {
        private readonly string _token;

        public BearerAthenticator(string token)
        {
            _token = token;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            request.AddHeader("Authorization", $"Bearer {_token}");
        }
    }
}
