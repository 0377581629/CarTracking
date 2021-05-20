using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Zero.Customize.Dto.EmailTemplate;

namespace Zero.Customize.Interfaces
{
    public interface IEmailTemplateAppService : IApplicationService 
    {
        Task<PagedResultDto<GetEmailTemplateForViewDto>> GetAll(GetAllEmailTemplateInput input);
        
        Task<GetEmailTemplateForEditOutput> GetEmailTemplateForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditEmailTemplateDto input);

		Task Delete(EntityDto input);
    }
}