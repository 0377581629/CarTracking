using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Cms.Application.Shared.Dto.ImageBlock;

namespace DPS.Cms.Application.Shared.Interfaces
{
    public interface IImageBlockAppService : IApplicationService 
    {
        Task<PagedResultDto<GetImageBlockForViewDto>> GetAll(GetAllImageBlockInput input);
        
        Task<GetImageBlockForEditOutput> GetImageBlockForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditImageBlockDto input);

		Task Delete(EntityDto input);
    }
}