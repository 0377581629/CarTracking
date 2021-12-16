using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using Zero.Authorization;
using Zero.Customize.Dto.CurrencyRate;
using Zero.Customize.Interfaces;

namespace Zero.Customize
{
    public class CurrencyRateAppService : ZeroAppServiceBase, ICurrencyRateAppService
    {
        #region Constructor

        private readonly IRepository<CurrencyRate> _currencyRateRepository;

        public CurrencyRateAppService(IRepository<CurrencyRate> currencyRateRepository)
        {
            _currencyRateRepository = currencyRateRepository;
        }

        #endregion

        public async Task<double?> GetLatestRate(string targetCurrency)
        {
            if (string.IsNullOrEmpty(targetCurrency))
                targetCurrency = "VND";
            return (await _currencyRateRepository.GetAll().OrderByDescending(o => o.Date).FirstOrDefaultAsync(o => o.TargetCurrency == targetCurrency))?.Rate;
        }
        
        private IQueryable<CurrencyRateDto> CurrencyRateQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from o in _currencyRateRepository.GetAll()
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => e.SourceCurrency.Contains(input.Filter) || e.TargetCurrency.Contains(input.Filter))
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
                select new CurrencyRateDto
                {
                    Id = o.Id,
                    Date = o.Date,
                    SourceCurrency = o.SourceCurrency,
                    TargetCurrency = o.TargetCurrency,
                    Rate = o.Rate
                };

            return query;
        }

        private class QueryInput
        {
            public GetAllCurrencyRateInput Input { get; set; }
            public int? Id { get; set; }
        }
        
        public async Task<PagedResultDto<GetCurrencyRateForViewDto>> GetAll(GetAllCurrencyRateInput input)
        {
            var queryInput = new QueryInput
            {
                Input = input
            };
            
            var objQuery = CurrencyRateQuery(queryInput);

            var pagedAndFilteredObjs = objQuery
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var objs = from o in pagedAndFilteredObjs
                select new GetCurrencyRateForViewDto
                {
                    CurrencyRate = o
                };

            var totalCount = await objQuery.CountAsync();

            return new PagedResultDto<GetCurrencyRateForViewDto>(
                totalCount,
                await objs.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.CurrencyRate_Edit)]
        public async Task<GetCurrencyRateForEditOutput> GetCurrencyRateForEdit(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };
            
            var objQuery = CurrencyRateQuery(queryInput);
            
            var obj = await objQuery.FirstOrDefaultAsync();

            var output = new GetCurrencyRateForEditOutput
            {
                CurrencyRate = ObjectMapper.Map<CreateOrEditCurrencyRateDto>(obj)
            };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCurrencyRateDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.CurrencyRate_Create)]
        protected virtual async Task Create(CreateOrEditCurrencyRateDto input)
        {
            var obj = ObjectMapper.Map<CurrencyRate>(input);
            await _currencyRateRepository.InsertAndGetIdAsync(obj);
        }

        [AbpAuthorize(AppPermissions.CurrencyRate_Edit)]
        protected virtual async Task Update(CreateOrEditCurrencyRateDto input)
        {
            if (input.Id.HasValue)
            {
                var obj = await _currencyRateRepository.FirstOrDefaultAsync((int) input.Id);
                ObjectMapper.Map(input, obj);    
            }
        }

        [AbpAuthorize(AppPermissions.CurrencyRate_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _currencyRateRepository.DeleteAsync(input.Id);
        }
    }
}