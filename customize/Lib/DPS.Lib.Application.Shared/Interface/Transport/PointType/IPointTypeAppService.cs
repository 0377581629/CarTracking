using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Lib.Application.Shared.Dto.Transport.PointType;

namespace DPS.Lib.Application.Shared.Interface.Transport.PointType
{
    public interface IPointTypeAppService: IApplicationService 
    {
        Task<PagedResultDto<GetPointTypeForViewDto>> GetAll(GetAllPointTypeInput input);
        
        Task<GetPointTypeForEditOutput> GetPointTypeForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditPointTypeDto input);

        Task Delete(EntityDto input);
    }
}