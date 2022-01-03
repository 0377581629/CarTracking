using Zero.Customize.Dto.Base;

namespace DPS.Cms.Application.Shared.Dto.Page
{
	public class PageDto : SimpleEntityDto
	{
		#region Theme + Layout
		
		public int? PageThemeId { get; set; }
        
		public string PageThemeCode { get; set; }
        
		public string PageThemeName { get; set; }
        
		public int PageLayoutId { get; set; }
        
		public string PageLayoutName { get; set; }
		
		#endregion
		
		public string Summary { get; set; }
        
		public string Content { get; set; }
        
		public string About { get; set; }
        
		public string Url { get; set; }
        
		public string Slug { get; set; }
        
		public bool IsHomePage { get; set; }

		public bool Publish { get; set; }
		
		#region SEO
        
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