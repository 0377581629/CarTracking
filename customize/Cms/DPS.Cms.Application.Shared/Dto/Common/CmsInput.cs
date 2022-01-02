using Abp.Application.Services.Dto;

namespace DPS.Cms.Application.Shared.Dto.Common
{
    public class CmsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

        public bool? HomePage { get; set; }
        
        public int? WidgetId { get; set; }
        
        public int? PageId { get; set; }
        
        public int? PageLayoutId { get; set; }
        
        public int? PageWidgetId { get; set; }
        
        public int? SupporterId { get; set; }
        
        public int? AdvertisementGroupId { get; set; }
        
        public string PageSlug { get; set; }
        
        public string ServiceTypeSlug { get; set; }
    }
}