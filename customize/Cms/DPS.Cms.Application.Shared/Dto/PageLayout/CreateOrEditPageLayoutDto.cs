using System.Collections.Generic;
using DPS.Cms.Application.Shared.Dto.Page;
using Zero.Customize.Dto.Base;

namespace DPS.Cms.Application.Shared.Dto.PageLayout
{
    public class CreateOrEditPageLayoutDto : SimpleCreateOrEditEntityDto
    {
        public int? PageThemeId { get; set; }
		
        public string PageThemeCode { get; set; }
		
        public string PageThemeName { get; set; }
    }
}