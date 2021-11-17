using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Zero.Abp.Payments.Dto;
using Zero.MultiTenancy.Payments;

namespace Zero.Abp.Payments
{
    public interface IUserPaymentAppService : IApplicationService
    {
        Task<long> CreatePayment(CreateUserPaymentDto input);

        Task CancelPayment(CancelUserPaymentDto input);

        Task<PagedResultDto<UserSubscriptionPaymentListDto>> GetPaymentHistory(GetPaymentHistoryInput input);

        Task<List<PaymentGatewayModel>> GetActiveGateways(GetActiveGatewaysInput input);

        Task<UserSubscriptionPaymentDto> GetPaymentAsync(long paymentId);

        Task<UserSubscriptionPaymentDto> GetLastCompletedPayment();

        Task ExtendSucceed(long paymentId);

        Task PaymentFailed(long paymentId);

        Task PaymentCancelled(long paymentId);
        
        Task<bool> HasAnyPayment();
    }
}
