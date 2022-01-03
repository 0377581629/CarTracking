using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Zero.Customize.Base;

namespace DPS.Cms.Core.Menu
{
    [Table("Cms_Menu_Group")]
    public class MenuGroup : SimpleEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
    }
}