using Garbacik.NetCore.Utilities.Restful.Extensions;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Garbacik.NetCore.Utilities.Restful.Authenticators;

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

    public ValueTask Authenticate(RestClient client, RestRequest request)
    {
        if (isAuthorizing)
            return ValueTask.CompletedTask;


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
        
        return ValueTask.CompletedTask;
    }

    public void MarkAsUnauthorized()
    {
        isAuthorized = false;
    }
}