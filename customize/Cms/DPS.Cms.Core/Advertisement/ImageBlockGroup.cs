using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Zero.Customize.Base;

namespace DPS.Cms.Core.Advertisement
{
    [Table("Cms_ImageBlock_Group")]
    public class ImageBlockGroup : SimpleFullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
    }
}