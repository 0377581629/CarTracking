using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Cms.Application.Shared.Dto.ImageBlockGroup;

namespace DPS.Cms.Application.Shared.Interfaces
{
    public interface IImageBlockGroupAppService : IApplicationService 
    {
        Task<PagedResultDto<GetImageBlockGroupForViewDto>> GetAll(GetAllImageBlockGroupInput input);
        
        Task<GetImageBlockGroupForEditOutput> GetImageBlockGroupForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditImageBlockGroupDto input);

		Task Delete(EntityDto input);
    }
}