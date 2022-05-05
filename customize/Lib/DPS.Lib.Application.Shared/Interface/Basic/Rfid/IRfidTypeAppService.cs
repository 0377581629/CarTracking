using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Lib.Application.Shared.Dto.Basic.Rfid.RfidType;

namespace DPS.Lib.Application.Shared.Interface.Basic.Rfid
{
    public interface IRfidTypeAppService: IApplicationService 
    {
        Task<PagedResultDto<GetRfidTypeForViewDto>> GetAll(GetAllRfidTypeInput input);
        
        Task<GetRfidTypeForEditOutput> GetRfidTypeForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditRfidTypeDto input);

        Task Delete(EntityDto input);
    }
}