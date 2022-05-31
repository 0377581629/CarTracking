using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Lib.Application.Shared.Dto.Transport.Car;

namespace DPS.Lib.Application.Shared.Interface.Transport.Car
{
    public interface ICarAppService: IApplicationService 
    {
        Task<PagedResultDto<GetCarForViewDto>> GetAll(GetAllCarInput input);
        
        Task<GetCarForEditOutput> GetCarForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCarDto input);

        Task Delete(EntityDto input);
    }
}