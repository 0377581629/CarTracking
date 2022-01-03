using Abp.Application.Services.Dto;

namespace DPS.Cms.Application.Shared.Dto.PageTheme
{
    public class GetAllPageThemeInput: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}