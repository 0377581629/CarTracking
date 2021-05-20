using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Zero.Common.Dto;
using Zero.Editions.Dto;

namespace Zero.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {
        Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false);

        Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input);

        GetDefaultEditionNameOutput GetDefaultEditionName();
    }
}