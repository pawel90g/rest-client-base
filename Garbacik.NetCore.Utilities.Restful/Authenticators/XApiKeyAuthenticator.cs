using Garbacik.NetCore.Utilities.Restful.Extensions;
using RestSharp;
using RestSharp.Authenticators;
using System.Collections.Generic;

namespace Garbacik.NetCore.Utilities.Restful.Authenticators
{
    public class XApiKeyAuthenticator : IAuthenticator
    {
        private readonly string xApiKey;

        public XApiKeyAuthenticator(string xApiKey)
        {
            this.xApiKey = xApiKey;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            request.PrepareHeaders(new Dictionary<string, string>
            {
                { "X-API-Key", xApiKey }
            });
        }
    }
}
