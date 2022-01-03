using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Zero.Customize.Base;

namespace DPS.Cms.Core.Menu
{
    [Table("Cms_Menu")]
    public class Menu: SimpleEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        
        public int MenuGroupId { get; set; }
        
        [ForeignKey("MenuGroupId")]
        public virtual MenuGroup MenuGroup { get; set; }

        public string Url { get; set; }
    }
}