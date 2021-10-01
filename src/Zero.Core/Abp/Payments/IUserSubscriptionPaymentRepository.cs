using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Zero.MultiTenancy.Payments;

namespace Zero.Abp.Payments
{
    public interface IUserSubscriptionPaymentRepository : IRepository<UserSubscriptionPayment, long>
    {
        Task<UserSubscriptionPayment> GetByGatewayAndPaymentIdAsync(SubscriptionPaymentGatewayType gateway, string paymentId);

        Task<UserSubscriptionPayment> GetLastCompletedPaymentOrDefaultAsync(long userId, SubscriptionPaymentGatewayType? gateway, bool? isRecurring);

        Task<UserSubscriptionPayment> GetLastPaymentOrDefaultAsync(long userId, SubscriptionPaymentGatewayType? gateway, bool? isRecurring);
    }
}
