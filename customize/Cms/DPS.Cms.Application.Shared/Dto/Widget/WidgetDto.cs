using Zero.Customize.Dto.Base;

namespace DPS.Cms.Application.Shared.Dto.Widget
{
	public class WidgetDto : SimpleEntityDto
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