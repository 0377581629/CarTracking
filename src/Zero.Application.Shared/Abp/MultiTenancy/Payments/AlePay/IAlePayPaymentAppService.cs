using System.Threading.Tasks;
using Abp.Application.Services;
using Zero.MultiTenancy.Payments.Dto;

namespace Zero.Abp.MultiTenancy.Payments.AlePay
{
    public interface IAlePayPaymentAppService : IApplicationService
    {
        Task<string> CreatePayment(AlePayCreatePaymentInput input);

        Task<string> CreateUserPayment(AlePayCreatePaymentInput input);
    }
}
