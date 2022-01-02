using System.Collections.Generic;
using Abp.Domain.Entities;
using Zero.Customize.Dto.Base;

namespace DPS.Cms.Application.Shared.Dto.PageLayout
{
	public class PageLayoutConfigDto : Entity
	{
		public string Name { get; set; }
		public List<PageLayoutBlockDto> Blocks { get; set; }
	}
}