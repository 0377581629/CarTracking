using System.Collections.Generic;
using DPS.Cms.Application.Shared.Dto.ImageBlock;
using Zero.Customize.Dto.Base;

namespace DPS.Cms.Application.Shared.Dto.ImageBlockGroup
{
	public class ImageBlockGroupDto : SimpleEntityDto
	{
		public List<ImageBlockDto> ListImageBlock { get; set; }
	}
}