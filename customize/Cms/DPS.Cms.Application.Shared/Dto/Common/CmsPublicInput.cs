using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace DPS.Cms.Application.Shared.Dto.Common
{
    public class CmsPublicInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
        
        public int? AdvertisementGroupId { get; set; }
        
        public int? WidgetId { get; set; }
        
        public int? PageId { get; set; }
        
        public int? PageWidgetId { get; set; }
        
        public string PageSlug { get; set; }
        
        public string ServiceTypeSlug { get; set; }
        
        public List<int> ImageBlockGroupIds { get; set; }
        
        public List<int> MenuGroupIds { get; set; }
    }
}