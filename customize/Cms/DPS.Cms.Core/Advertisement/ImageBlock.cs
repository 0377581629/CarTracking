using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Zero.Customize.Base;

namespace DPS.Cms.Core.Advertisement
{
    [Table("Cms_ImageBlock")]
    public class ImageBlock : SimpleFullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        
        public int ImageBlockGroupId { get; set; }
        
        [ForeignKey("ImageBlockGroupId")]
        public virtual ImageBlockGroup ImageBlockGroup { get; set; }
        
        public string Image { get; set; }
        
        public string ImageMobile { get; set; }
        
        public string TargetUrl { get; set; }
    }
}