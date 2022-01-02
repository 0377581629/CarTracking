using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Cms.Application.Shared.Dto.Tags;

namespace DPS.Cms.Application.Shared.Interfaces
{
    public interface ITagsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetTagsForViewDto>> GetAll(GetAllTagsInput input);
        
        Task<GetTagsForEditOutput> GetTagsForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditTagsDto input);

		Task Delete(EntityDto input);
    }
}