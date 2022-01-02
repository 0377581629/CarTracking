using System.Collections.Generic;
using Abp.Application.Services.Dto;
using DPS.Cms.Application.Shared.Dto.PageLayout;
using Zero.Customize.Dto.Base;

namespace DPS.Cms.Application.Shared.Dto.Page
{
    public class PageConfigDto: EntityDto
    {
        public string Name { get; set; }

        public List<PageLayoutBlockDto> Blocks { get; set; }
        
        public List<PageWidgetDto> Widgets { get; set; }
    }
}