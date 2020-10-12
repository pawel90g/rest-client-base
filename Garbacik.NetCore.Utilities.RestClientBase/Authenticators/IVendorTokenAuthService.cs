using System.Threading.Tasks;

namespace Garbacik.NetCore.Utilities.RestClientBase.Authenticators
{
    public interface IVendorTokenAuthService
    {
        Task<string> AuthorizeAsync();
    }
}
