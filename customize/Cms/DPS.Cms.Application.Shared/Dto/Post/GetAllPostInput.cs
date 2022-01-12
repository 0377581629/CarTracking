using Abp.Application.Services.Dto;

namespace DPS.Cms.Application.Shared.Dto.Post
{
    public class GetAllPostInput: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        
        public int? CategoryId { get; set; }
    }
}