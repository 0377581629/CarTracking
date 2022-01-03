using System;
using Abp.AutoMapper;
using Zero.MultiTenancy;

namespace Zero.Customize.Cache
{
    [Serializable]
    [AutoMapFrom(typeof(Tenant))]
    public class CustomTenantCacheItem
    {
        public const string CacheName = "AbpZeroCustomTenantCache";

        public const string ByNameCacheName = "AbpZeroCustomTenantByNameCache";
        
        public const string ByDomainCacheName = "AbpZeroCustomTenantByDomainCache";
        
        public int Id { get; set; }

        public string Name { get; set; }

        public string TenancyName { get; set; }

        public string ConnectionString { get; set; }

        public int? EditionId { get; set; }

        public bool IsActive { get; set; }

        public object CustomData { get; set; }
        
        #region Extent Base

        public int? ParentId { get; set; }
        
        public string Code { get; set; }

        public string WebTitle { get; set; }

        public string WebDescription { get; set; }

        public string WebAuthor { get; set; }

        public string WebKeyword { get; set; }

        public string WebFavicon { get; set; }

        public string Domain { get; set; }
        
        #endregion
        
        #region Extent UI

        public virtual Guid? LoginLogoId { get; set; }

        public virtual Guid? MenuLogoId { get; set; }

        public virtual Guid? LoginBackgroundId { get; set; }
        
        public virtual string LoginLogoFileType { get; set; }

        public virtual string MenuLogoFileType { get; set; }

        public virtual string LoginBackgroundFileType { get; set; }
        
        #endregion
    }
}