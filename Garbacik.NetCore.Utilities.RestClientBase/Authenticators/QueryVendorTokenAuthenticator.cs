using RestSharp;
using RestSharp.Authenticators;
using System;

namespace Garbacik.NetCore.Utilities.RestClientBase.Authenticators
{
    public class QueryVendorTokenAuthenticator : IAuthenticator, IUnauthorizedClient
    {
        private readonly IVendorTokenAuthService vendorTokenAuthService;
        private readonly string tokenParamName;

        private bool isAuthorizing = false;
        private bool isAuthorized = false;

        private string bearerToken;

        public QueryVendorTokenAuthenticator(IVendorTokenAuthService vendorTokenAuthService, string tokenParamName)
        {
            this.tokenParamName = tokenParamName;
            this.vendorTokenAuthService = vendorTokenAuthService;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            if (isAuthorizing)
                return;


            if (!isAuthorized)
            {
                isAuthorizing = true;
                try
                {
                    bearerToken = vendorTokenAuthService
                        .AuthorizeAsync()
                        .GetAwaiter()
                        .GetResult();

                    isAuthorized = true;
                }
                catch (Exception e)
                {
                    throw;
                }
                finally
                {
                    isAuthorizing = false;
                }
            }

            request.AddParameter(tokenParamName, bearerToken, ParameterType.QueryStringWithoutEncode);

            isAuthorized = true;
        }

        public void MarkAsUnauthorized()
        {
            isAuthorized = false;
        }
    }
}
