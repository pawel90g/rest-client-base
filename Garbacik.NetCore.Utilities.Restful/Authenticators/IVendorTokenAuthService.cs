using System.Threading.Tasks;

namespace Garbacik.NetCore.Utilities.Restful.Authenticators
{
    public interface IVendorTokenAuthService
    {
        Task<string> AuthorizeAsync();
    }
}
