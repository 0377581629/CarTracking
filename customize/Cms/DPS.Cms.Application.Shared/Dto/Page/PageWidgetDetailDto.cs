using Abp.Application.Services.Dto;
using DPS.Cms.Application.Shared.Interfaces.Common;

namespace DPS.Cms.Application.Shared.Dto.Page
{
	public class PageWidgetDetailDto : EntityDto<int?>, IMayHaveImageBlockGroupInfo, IMayHaveMenuGroupInfo
	{
		public int PageWidgetId { get; set; }
		
		#region Image Block Group
		public int? ImageBlockGroupId { get; set; }
		public string ImageBlockGroupCode { get; set; }
		public string ImageBlockGroupName { get; set; }
		#endregion
		
		#region Menu Group
		public int? MenuGroupId { get; set; }
		public string MenuGroupCode { get; set; }
		public string MenuGroupName { get; set; }
		#endregion

		public string CustomContent { get; set; }
	}
}