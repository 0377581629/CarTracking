using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Lib.Application.Shared.Dto.Transport.Route;

namespace DPS.Lib.Application.Shared.Interface.Transport.Route
{
    public interface IRouteAppService: IApplicationService 
    {
        Task<PagedResultDto<GetRouteForViewDto>> GetAll(GetAllRouteInput input);
        
        Task<GetRouteForEditOutput> GetRouteForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditRouteDto input);

        Task Delete(EntityDto input);
    }
}