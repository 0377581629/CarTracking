using Abp.AutoMapper;
using DPS.Cms.Core.Shared;
using Zero.Customize.Dto.Base;

namespace DPS.Cms.Application.Shared.Dto.Category
{
	[AutoMapFrom(typeof(Core.Post.Category))]
	public class CategoryDto : SimpleEntityDto, IHaveSeoInfo
	{
		public int? ParentId { get; set; }
		
		public string ParentCode { get; set; }
		
		public string ParentCategoryCode { get; set; }
		
		public string ParentName { get; set; }
		
		public string CategoryCode { get; set; }
		
		public string About { get; set; }
        
		public string Url { get; set; }
        
		public string Slug { get; set; }
        
		public string Image { get; set; }
		
		public int PostCount { get; set; }
        
		public int CommentCount { get; set; }
        
		public int ViewCount { get; set; }
		
		#region SEO Info
		public bool TitleDefault { get; set; }
		public string Title { get; set; }
		public bool DescriptionDefault { get; set; }
		public string Description { get; set; }
		public bool KeywordDefault { get; set; }
		public string Keyword { get; set; }
		public bool AuthorDefault { get; set; }
		public string Author { get; set; }
		#endregion
	}
}