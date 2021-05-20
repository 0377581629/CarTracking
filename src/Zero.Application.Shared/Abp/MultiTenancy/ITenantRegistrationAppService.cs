using System.Threading.Tasks;
using Abp.Application.Services;
using Zero.Editions.Dto;
using Zero.MultiTenancy.Dto;

namespace Zero.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}