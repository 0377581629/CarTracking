using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Lib.Application.Shared.Dto.Transport.AssignmentRoute;

namespace DPS.Lib.Application.Shared.Interface.Transport.AssignmentRoute
{
    public interface IAssignmentRouteAppService: IApplicationService 
    {
        Task<PagedResultDto<GetAssignmentRouteForViewDto>> GetAll(GetAllAssignmentRouteInput input);
        
        Task<GetAssignmentRouteForEditOutput> GetAssignmentRouteForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditAssignmentRouteDto input);

        Task Delete(EntityDto input);
    }
}