using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Zero.Customize.Base;

namespace DPS.Cms.Core.Post
{
    [Table("Cms_Tags")]
    public class Tags : SimpleFullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        
        public int ViewCount { get; set; }
    }
}