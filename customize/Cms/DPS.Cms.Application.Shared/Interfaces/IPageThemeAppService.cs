using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Cms.Application.Shared.Dto.PageTheme;

namespace DPS.Cms.Application.Shared.Interfaces
{
    public interface IPageThemeAppService: IApplicationService 
    {
        Task<PagedResultDto<GetPageThemeForViewDto>> GetAll(GetAllPageThemeInput input);
        
        Task<GetPageThemeForEditOutput> GetPageThemeForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditPageThemeDto input);

        Task Delete(EntityDto input);
    }
}