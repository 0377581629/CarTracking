using System.Linq;
using System.Threading.Tasks;
using Abp.EntityFrameworkCore;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using Zero.Abp.Payments;
using Zero.EntityFrameworkCore;
using Zero.EntityFrameworkCore.Repositories;

namespace Zero.MultiTenancy.Payments
{
    public class UserSubscriptionPaymentRepository : ZeroRepositoryBase<UserSubscriptionPayment, long>, IUserSubscriptionPaymentRepository
    {
        public UserSubscriptionPaymentRepository(IDbContextProvider<ZeroDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<UserSubscriptionPayment> GetByGatewayAndPaymentIdAsync(SubscriptionPaymentGatewayType gateway, string paymentId)
        {
            return await SingleAsync(p => p.ExternalPaymentId == paymentId && p.Gateway == gateway);
        }

        public async Task<UserSubscriptionPayment> GetLastCompletedPaymentOrDefaultAsync(long userId, SubscriptionPaymentGatewayType? gateway, bool? isRecurring)
        {
            return (await GetAll()
                    .Where(p => p.UserId == userId)
                    .Where(p => p.Status == SubscriptionPaymentStatus.Completed)
                    .WhereIf(gateway.HasValue, p => p.Gateway == gateway.Value)
                    .WhereIf(isRecurring.HasValue, p => p.IsRecurring == isRecurring.Value)
                    .ToListAsync()
                )
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();
        }

        public async Task<UserSubscriptionPayment> GetLastPaymentOrDefaultAsync(long userId, SubscriptionPaymentGatewayType? gateway, bool? isRecurring)
        {
            return (await GetAll()
                    .Where(p => p.UserId == userId)
                    .WhereIf(gateway.HasValue, p => p.Gateway == gateway.Value)
                    .WhereIf(isRecurring.HasValue, p => p.IsRecurring == isRecurring.Value)
                    .ToListAsync()).OrderByDescending(x => x.Id
                )
                .FirstOrDefault();
        }
    }
}
