using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Zero.Abp.Authorization.Users.Dto;
using Zero.Abp.Authorization.Users.Payments.Dto;
using Zero.MultiTenancy.Payments;
using Zero.MultiTenancy.Payments.Dto;

namespace Zero.Abp.Authorization.Users.Payments
{
    public interface IUserPaymentAppService : IApplicationService
    {
        Task<long> CreatePayment(CreateUserPaymentDto input);

        Task CancelPayment(CancelUserPaymentDto input);

        Task<PagedResultDto<UserSubscriptionPaymentListDto>> GetPaymentHistory(GetUserPaymentHistoryInput input);

        List<PaymentGatewayModel> GetActiveGateways(GetActiveGatewaysInput input);

        Task<UserSubscriptionPaymentDto> GetPaymentAsync(long paymentId);

        Task<UserSubscriptionPaymentDto> GetLastCompletedPayment();

        Task ExtendSucceed(long paymentId);

        Task PaymentFailed(long paymentId);

        Task<bool> HasAnyPayment();
    }
}
