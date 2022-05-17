using Abp.IdentityServer4vNext;
using Abp.Zero.EntityFrameworkCore;
using DPS.Cms.Core.Advertisement;
using DPS.Cms.Core.Menu;
using DPS.Cms.Core.Page;
using DPS.Cms.Core.Post;
using DPS.Cms.Core.Widget;
using DPS.Lib.Core.Basic.Driver;
using DPS.Lib.Core.Basic.Rfid;
using DPS.Lib.Core.Basic.Technician;
using DPS.Lib.Core.Basic.Treasurer;
using Microsoft.EntityFrameworkCore;
using Zero.Abp.Authorization.Accounting;
using Zero.Abp.Payments;
using Zero.Authorization.Delegation;
using Zero.Authorization.Roles;
using Zero.Authorization.Users;
using Zero.Chat;
using Zero.Customize;
using Zero.Customize.Dashboard;
using Zero.Editions;
using Zero.Friendships;
using Zero.MultiTenancy;
using Zero.MultiTenancy.Accounting;
using Zero.MultiTenancy.Payments;
using Zero.Storage;

namespace Zero.EntityFrameworkCore
{
    public class ZeroDbContext : AbpZeroDbContext<Tenant, Role, User, ZeroDbContext>, IAbpPersistedGrantDbContext
    {
        #region Zero
        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public virtual DbSet<UserSubscriptionPayment> UserSubscriptionPayments { get; set; }
        #endregion
        
        #region Cms
        public virtual DbSet<ImageBlockGroup> ImageBlockGroups { get; set; }
        public virtual DbSet<ImageBlock> ImageBlocks { get; set; }
        
        public virtual DbSet<PageTheme> PageThemes { get; set; }
        public virtual DbSet<PageLayout> PageLayouts { get; set; }
        public virtual DbSet<PageLayoutBlock> PageLayoutBlocks { get; set; }
        
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<PageWidget> PageWidgets { get; set; }
        public virtual DbSet<PageWidgetDetail> PageWidgetDetails { get; set; }
        
        public virtual DbSet<Widget> Widgets { get; set; }
        
        public virtual DbSet<MenuGroup> MenuGroups { get; set; }
        
        public virtual DbSet<Menu> Menus { get; set; }
        
        public virtual DbSet<WidgetPageTheme> WidgetPageThemes { get; set; }
        
        public virtual DbSet<Category> Categories { get; set; }
        
        public virtual DbSet<Post> Posts { get; set; }
        
        public virtual DbSet<PostCategoryDetail> PostCategoryDetails { get; set; }
        
        public virtual DbSet<PostTagDetail> PostTagDetails { get; set; }
        
        public virtual DbSet<Tags> Tags { get; set; }
        #endregion

        #region Lib

        public virtual DbSet<RfidType> RfidTypes { get; set; }
        
        public virtual DbSet<Technician> Technicians { get; set; }
        
        public virtual DbSet<Treasurer> Treasurers { get; set; }
        
        public virtual DbSet<Driver> Drivers { get; set; }

        #endregion
        
        #region Abp Customize

        public virtual DbSet<DashboardWidget> DashboardWidgets { get; set; }
        public virtual DbSet<EditionPermission> EditionPermissions { get; set; }
        public virtual DbSet<EditionDashboardWidget> EditionDashboardWidgets { get; set; }
        public virtual DbSet<RoleDashboardWidget> RoleDashboardWidgets { get; set; }
        public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }
        public virtual DbSet<CurrencyRate> CurrencyRates { get; set; }
        public virtual DbSet<UserInvoice> UserInvoices { get; set; }
        
        #endregion
        
        public ZeroDbContext(DbContextOptions<ZeroDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique();
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}
