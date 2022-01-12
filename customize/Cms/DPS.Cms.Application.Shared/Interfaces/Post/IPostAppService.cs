using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto; 
using DPS.Cms.Application.Shared.Dto.Post;

namespace DPS.Cms.Application.Shared.Interfaces
{
    public interface IPostAppService: IApplicationService 
    {
        Task<PagedResultDto<GetPostForViewDto>> GetAll(GetAllPostInput input);
        
        Task<GetPostForEditOutput> GetPostForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditPostDto input);
        
        Task UpdateStatus(EntityDto input);

        Task Delete(EntityDto input);
    }
}