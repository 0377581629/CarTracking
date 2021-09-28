using System;
using System.Globalization;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Threading;
using Fixer.dotnet;
using Fixer.dotnet.Abstractions;
using Z.EntityFramework.Extensions;

namespace Zero.Customize.BackgroundJobs
{
    public class CurrencyRateBackgroundJob : ZeroDomainServiceBase, ICurrencyRateBackgroundJob
    {
        #region Constructor
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<CurrencyRate> _currencyRateRepository;
        private readonly IExchangeRatesSource _exchangeRatesSource;
        public CurrencyRateBackgroundJob(IRepository<CurrencyRate> currencyRateRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _currencyRateRepository = currencyRateRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _exchangeRatesSource = new ExchangeRatesSource();
        }

        #endregion

        public void UpdateRates()
        {
            using var unitOfWork = _unitOfWorkManager.Begin();
            var fixerExchange = AsyncHelper.RunSync(()=>_exchangeRatesSource.GetCurrenciesAsync("28493e2ae00af25679d4b66ef2a5a66b", Currencies.USDollar));
            if (!fixerExchange.Success) return;
            
            var latestCurrencyRate = _currencyRateRepository.GetAll().OrderByDescending(o => o.Date).FirstOrDefault();
            if (latestCurrencyRate != null && DateTimeHelper.UnixTimeStampToDateTime(Convert.ToDouble(fixerExchange.Timestamp)) <= latestCurrencyRate.Date) return;
            var newCurrencyRates = fixerExchange.Rates.Select(rate => new CurrencyRate { Date = DateTimeHelper.UnixTimeStampToDateTime(Convert.ToDouble(fixerExchange.Timestamp)), SourceCurrency = "USD", TargetCurrency = rate.Key, Rate = double.Parse(rate.Value, CultureInfo.InvariantCulture) }).ToList();
            EntityFrameworkManager.ContextFactory = _ => _currencyRateRepository.GetDbContext();    
            AsyncHelper.RunSync(()=> _currencyRateRepository.GetDbContext().BulkInsertAsync(newCurrencyRates));
            unitOfWork.Complete();
        }
    }
}