using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Threading.Tasks;

namespace Garbacik.NetCore.Utilities.Restful.Authenticators;

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

        request.AddParameter(tokenParamName, bearerToken, ParameterType.QueryString);

        isAuthorized = true;
        
        return ValueTask.CompletedTask;
    }

    public void MarkAsUnauthorized()
    {
        isAuthorized = false;
    }
}