using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Lib.Application.Shared.Dto.Transport.Point;

namespace DPS.Lib.Application.Shared.Interface.Transport.Point
{
    public interface IPointAppService: IApplicationService 
    {
        Task<PagedResultDto<GetPointForViewDto>> GetAll(GetAllPointInput input);
        
        Task<GetPointForEditOutput> GetPointForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditPointDto input);

        Task Delete(EntityDto input);
    }
}