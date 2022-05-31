using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Lib.Application.Shared.Dto.Basic.Device;

namespace DPS.Lib.Application.Shared.Interface.Basic.Device
{
    public interface IDeviceAppService: IApplicationService 
    {
        Task<PagedResultDto<GetDeviceForViewDto>> GetAll(GetAllDeviceInput input);
        
        Task<GetDeviceForEditOutput> GetDeviceForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditDeviceDto input);

        Task Delete(EntityDto input);
    }
}