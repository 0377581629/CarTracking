using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using DPS.Cms.Core.Page;

namespace DPS.Cms.Core.Widget
{
    [Table("Cms_Widget_PageTheme")]
    public class WidgetPageTheme : Entity
    {
        public int WidgetId { get; set; }
        
        [ForeignKey("WidgetId")]
        public virtual Core.Widget.Widget Widget { get; set; }
        
        public int PageThemeId { get; set; }
        
        [ForeignKey("PageThemeId")]
        public virtual PageTheme PageTheme { get; set; }
    }
}