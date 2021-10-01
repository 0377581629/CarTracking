using System.Threading.Tasks;
using Abp.Application.Services;
using JetBrains.Annotations;

namespace Zero.Customize.Interfaces
{
    public interface ICurrencyRateAppService : IApplicationService
    {
        Task<double?> GetLatestRate(string targetCurrency = "VND");
    }
}