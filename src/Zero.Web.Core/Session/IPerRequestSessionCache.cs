using System.Threading.Tasks;
using Zero.Sessions.Dto;

namespace Zero.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
