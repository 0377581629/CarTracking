using Abp.Application.Services.Dto;

namespace DPS.Cms.Application.Shared.Dto.Widget
{
    public class WidgetPageThemeDto : EntityDto<int?>
    {
        public int WidgetId { get; set; }
        
        public string WidgetCode { get; set; }
        
        public string WidgetName { get; set; }
        
        public int PageThemeId { get; set; }
        
        public string PageThemeCode { get; set; }
        
        public string PageThemeName { get; set; }
    }
}