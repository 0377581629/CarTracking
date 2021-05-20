using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace Zero.Customize.Dashboard
{
    [Table("AbpEditionDashboardWidgets")]
    public class EditionDashboardWidget : FullAuditedEntity
    {
        public int EditionId { get; set; }
        
        public int DashboardWidgetId { get; set; }
    }
}