using System.Collections.Generic;
using DPS.Cms.Application.Shared.Dto.Page;
using DPS.Cms.Application.Shared.Dto.PageLayout;

namespace Zero.Web.Areas.Cms.Models.Page
{
    public class PageBlockWidgetViewModel
    {
        public PageLayoutBlockDto Block { get; set; }
        
		public List<PageWidgetDto> Widgets { get; set; }

        public PageBlockWidgetViewModel(PageLayoutBlockDto block, List<PageWidgetDto> widgets)
        {
            Block = block;
            Widgets = widgets;
        }
    }
}