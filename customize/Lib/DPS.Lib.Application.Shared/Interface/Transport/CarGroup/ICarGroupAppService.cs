using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Lib.Application.Shared.Dto.Transport.CarGroup;

namespace DPS.Lib.Application.Shared.Interface.Transport.CarGroup
{
    public interface ICarGroupAppService: IApplicationService 
    {
        Task<PagedResultDto<GetCarGroupForViewDto>> GetAll(GetAllCarGroupInput input);
        
        Task<GetCarGroupForEditOutput> GetCarGroupForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCarGroupDto input);

        Task Delete(EntityDto input);
    }
}