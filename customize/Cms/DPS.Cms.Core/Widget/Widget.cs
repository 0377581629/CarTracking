using System.ComponentModel.DataAnnotations.Schema;
using Zero.Customize.Base;

namespace DPS.Cms.Core.Widget
{
    [Table("Cms_Widget")]
    public class Widget : SimpleFullAuditedEntity
    {
        public string About { get; set; }
        
        public string ActionName { get; set; }
        
        public string ControllerName { get; set; }
        
        public string JsBundleUrl { get; set; }
        
        public string JsScript { get; set; }
        
        public string JsPlain { get; set; }
        
        public string CssBundleUrl { get; set; }
        
        public string CssScript { get; set; }
        
        public string CssPlain { get; set; }
        
        public int ContentType { get; set; }
        
        public int ContentCount { get; set; }
        
        public bool AsyncLoad { get; set; }
    }
}