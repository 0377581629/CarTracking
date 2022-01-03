using System.Collections.Generic;
using Abp.Application.Services.Dto;
using DPS.Cms.Application.Shared.Dto.ImageBlock;
using DPS.Cms.Application.Shared.Dto.ImageBlockGroup;
using DPS.Cms.Application.Shared.Dto.Menu;
using DPS.Cms.Application.Shared.Dto.MenuGroup;
using Zero.Authorization.Users.Dto;

namespace DPS.Cms.Application.Shared.Dto.Page
{
	public class PageWidgetDto : EntityDto<int?>
	{
		public string UniqueId { get; set; }
		
		public string PageBlockColumnId { get; set; }
		
		public int PageId { get; set; }
		
		public int? PageThemeId { get; set; }
		
		public string PageThemeCode { get; set; }
		
		public string PageThemeName { get; set; }
        
		#region Widget
		
		public int? WidgetPageThemeId { get; set; }
		
		public string WidgetPageThemeCode { get; set; }
		
		public string WidgetPageThemeName { get; set; }
		
		public int WidgetId { get; set; }
		
		public string WidgetName { get; set; }
		
		public string WidgetActionName { get; set; }
		
		public string WidgetControllerName { get; set; }
		
		public int WidgetContentType { get; set; }
		
		public int WidgetContentCount { get; set; }
		
		public string WidgetJsBundleUrl { get; set; }
        
		public string WidgetJsScript { get; set; }
        
		public string WidgetJsPlain { get; set; }
        
		public string WidgetCssBundleUrl { get; set; }
        
		public string WidgetCssScript { get; set; }
        
		public string WidgetCssPlain { get; set; }
		
		public int Order { get; set; }
		
		public List<PageWidgetDetailDto> Details { get; set; }
		#endregion
		
		#region Use for front pages
		
		public List<ImageBlockGroupDto> ListImageBlockGroup { get; set; }

		public List<MenuGroupDto> ListMenuGroup { get; set; }
		
		#endregion
	}
}