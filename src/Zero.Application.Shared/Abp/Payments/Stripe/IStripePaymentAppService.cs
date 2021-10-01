using System.Threading.Tasks;
using Abp.Application.Services;
using Zero.Abp.Payments.Dto;
using Zero.Abp.Payments.Stripe.Dto;

namespace Zero.Abp.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}