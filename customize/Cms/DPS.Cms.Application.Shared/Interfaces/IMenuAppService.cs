using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Cms.Application.Shared.Dto.Menu;

namespace DPS.Cms.Application.Shared.Interfaces
{
    public interface IMenuAppService: IApplicationService 
    {
        Task<PagedResultDto<GetMenuForViewDto>> GetAll(GetAllMenuInput input);
        
        Task<GetMenuForEditOutput> GetMenuForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditMenuDto input);

        Task Delete(EntityDto input);
    }
}