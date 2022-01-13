using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Cms.Application.Shared.Dto.MenuGroup;

namespace DPS.Cms.Application.Shared.Interfaces.Menu
{
    public interface IMenuGroupAppService: IApplicationService 
    {
        Task<PagedResultDto<GetMenuGroupForViewDto>> GetAll(GetAllMenuGroupInput input);
        
        Task<GetMenuGroupForEditOutput> GetMenuGroupForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditMenuGroupDto input);

        Task Delete(EntityDto input);
    }
}