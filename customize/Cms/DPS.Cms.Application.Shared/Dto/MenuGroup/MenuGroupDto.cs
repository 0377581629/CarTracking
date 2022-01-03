using System.Collections.Generic;
using DPS.Cms.Application.Shared.Dto.Menu;
using Zero.Customize.Dto.Base;

namespace DPS.Cms.Application.Shared.Dto.MenuGroup
{
    public class MenuGroupDto: SimpleEntityDto
    {
        public List<MenuDto> ListMenu { get; set; }
    }
}