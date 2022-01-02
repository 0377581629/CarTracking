using System.Collections.Generic;
using Zero.Customize.Dto.Base;

namespace DPS.Cms.Application.Shared.Dto.PageLayout
{
	public class PageLayoutDto : SimpleEntityDto
	{
		public int? PageThemeId { get; set; }
		
		public string PageThemeCode { get; set; }
		
		public string PageThemeName { get; set; }
		
		public List<PageLayoutBlockDto> Blocks { get; set; }
	}
}