using Abp.Application.Services.Dto;

namespace DPS.Cms.Application.Shared.Dto.Common
{
    public class GetPageInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
        
        public int? PageId { get; set; }

        public string PageSlug { get; set; }
        
        public bool? HomePage { get; set; }
    }
}