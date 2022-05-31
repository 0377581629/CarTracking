using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Lib.Application.Shared.Dto.Transport.CarType;

namespace DPS.Lib.Application.Shared.Interface.Transport.CarType
{
    public interface ICarTypeAppService: IApplicationService 
    {
        Task<PagedResultDto<GetCarTypeForViewDto>> GetAll(GetAllCarTypeInput input);
        
        Task<GetCarTypeForEditOutput> GetCarTypeForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCarTypeDto input);

        Task Delete(EntityDto input);
    }
}