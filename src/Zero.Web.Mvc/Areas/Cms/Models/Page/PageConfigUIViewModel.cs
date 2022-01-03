using System.Collections.Generic;
using DPS.Cms.Application.Shared.Dto.Page;
using DPS.Cms.Application.Shared.Dto.Widget;

namespace Zero.Web.Areas.Cms.Models.Page
{
    public class PageConfigUIViewModel
    {
        public PageConfigDto PageConfig { get; set; }
        
		public List<WidgetDto> AvailableWidgets { get; set; }
    }
}