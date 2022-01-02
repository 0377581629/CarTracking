using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using DPS.Cms.Core.Advertisement;
using DPS.Cms.Core.Menu;
using JetBrains.Annotations;

namespace DPS.Cms.Core.Page
{
    [Table("Cms_Page_Widget_Detail")]
    public class PageWidgetDetail : Entity
    {
        public string UniqueId { get; set; }
        
        public int PageWidgetId { get; set; }
        
        [ForeignKey("PageWidgetId")]
        public virtual PageWidget PageWidget { get; set; }

        public int? ImageBlockGroupId { get; set; }
        
        [ForeignKey("ImageBlockGroupId")]
        [CanBeNull]
        public virtual ImageBlockGroup ImageBlockGroup { get; set; }
        
        public int? MenuGroupId { get; set; }
        
        [ForeignKey("MenuGroupId")]
        [CanBeNull]
        public virtual MenuGroup MenuGroup { get; set; }
        
        public string CustomContent { get; set; }
    }
}