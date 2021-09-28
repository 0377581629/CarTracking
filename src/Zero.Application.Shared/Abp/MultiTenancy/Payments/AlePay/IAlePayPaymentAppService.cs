using System.Threading.Tasks;
using Abp.Application.Services;

namespace Zero.Abp.MultiTenancy.Payments.AlePay
{
    public interface IAlePayPaymentAppService : IApplicationService
    {
        Task<string> CreatePayment(long paymentId);

        Task<string> CreateUserPayment(long paymentId);
    }
}
