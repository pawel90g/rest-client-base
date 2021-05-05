using RestSharp;
using RestSharp.Authenticators;

namespace Garbacik.NetCore.Utilities.Restful.Authenticators
{
    public class AuthorizationHeaderAuthenticator : IAuthenticator
    {
        private readonly string authorizationHeaderValue;
        
        public AuthorizationHeaderAuthenticator(string authorizationHeaderValue)
        {
            this.authorizationHeaderValue = authorizationHeaderValue;
        }
        
        public void Authenticate(IRestClient client, IRestRequest request)
        {
            request.AddHeader("Authorization", authorizationHeaderValue);
        }
    }
}