using System.Threading.Tasks;
using Abp.Application.Services;
using Zero.MultiTenancy.Payments.Dto;
using Zero.MultiTenancy.Payments.Stripe.Dto;

namespace Zero.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}