using System.Threading.Tasks;
using Abp.Application.Services;
using Zero.Sessions.Dto;

namespace Zero.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
