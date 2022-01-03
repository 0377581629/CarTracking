using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace DPS.Cms.Core.Page
{
    [Table("Cms_Page_Widget")]
    public class PageWidget : Entity
    {
        public string UniqueId { get; set; }
        
        public string PageBlockColumnId { get; set; }
        
        public int PageId { get; set; }
        
        [ForeignKey("PageId")]
        public virtual Core.Page.Page Page { get; set; }
        
        public int WidgetId { get; set; }
        
        [ForeignKey("WidgetId")]
        public virtual Widget.Widget Widget { get; set; }
        
        public int Order { get; set; }
    }
}