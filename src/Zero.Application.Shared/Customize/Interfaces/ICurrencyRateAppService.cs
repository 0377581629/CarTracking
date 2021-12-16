using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using JetBrains.Annotations;
using Zero.Customize.Dto.CurrencyRate;

namespace Zero.Customize.Interfaces
{
    public interface ICurrencyRateAppService : IApplicationService
    {
        Task<double?> GetLatestRate(string targetCurrency = "VND");
        
        Task<PagedResultDto<GetCurrencyRateForViewDto>> GetAll(GetAllCurrencyRateInput input);
        
        Task<GetCurrencyRateForEditOutput> GetCurrencyRateForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCurrencyRateDto input);

        Task Delete(EntityDto input);
    }
}