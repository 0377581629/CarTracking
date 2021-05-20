using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Zero.Customize.Dashboard
{
    [Table("AbpRoleDashboardWidgets")]
    public class RoleDashboardWidget : Entity
    {
        public int RoleId { get; set; }
        
        public int DashboardWidgetId { get; set; }
    }
}