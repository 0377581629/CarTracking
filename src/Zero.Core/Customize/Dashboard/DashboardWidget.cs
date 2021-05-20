using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Zero.Customize.Dashboard
{
    [Table("AbpDashboardWidgets")]
    public class DashboardWidget : Entity
    {
        public string WidgetId { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public byte Height { get; set; }

        public byte Width { get; set; }

        public byte PositionX { get; set; }

        public byte PositionY { get; set; }
        
        public string ViewName { get; set; }
        
        public string JsPath { get; set; }
        
        public string CssPath { get; set; }

        public string Filters { get; set; }
        
        public bool IsDefault { get; set; }
    }
}