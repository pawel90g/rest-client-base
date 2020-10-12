using Garbacik.NetCore.Utilities.RestClientBase.Extensions;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;

namespace Garbacik.NetCore.Utilities.RestClientBase.Authenticators
{
    public class VendorTokenAuthenticator : IAuthenticator, IUnauthorizedClient
    {
        private readonly IVendorTokenAuthService vendorTokenAuthService;
        private bool isAuthorizing = false;
        private bool isAuthorized = false;

        private string bearerToken;

        public VendorTokenAuthenticator(IVendorTokenAuthService vendorTokenAuthService)
        {
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

            request.PrepareHeaders(new Dictionary<string, string>
            {
                { "Authorization", $"Bearer {bearerToken}" },
            });

            isAuthorized = true;
        }

        public void MarkAsUnauthorized()
        {
            isAuthorized = false;
        }
    }
}
