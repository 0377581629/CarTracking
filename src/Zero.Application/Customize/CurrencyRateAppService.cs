using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Zero.Customize.Interfaces;

namespace Zero.Customize
{
    [AbpAuthorize]
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
    }
}