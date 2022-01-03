using System;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Zero.MultiTenancy.Dto
{
    public class TenantListDto : EntityDto, IPassivable, IHasCreationTime
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }

        public string EditionDisplayName { get; set; }

        [DisableAuditing]
        public string ConnectionString { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? SubscriptionEndDateUtc { get; set; }

        public int? EditionId { get; set; }

        public bool IsInTrialPeriod { get; set; }
        
        #region Extent
        public string Code { get; set; }
        public int? ParentId { get; set; }
        
        public string ParentTenancyName { get; set; }
        
        public string ParentName { get; set; }
        public bool Selected { get; set; }
        
        public Guid? LoginLogoId { get; set; }

        public Guid? MenuLogoId { get; set; }

        public Guid? LoginBackgroundId { get; set; }
        
        public string Domain { get; set; }
        #endregion
    }
}