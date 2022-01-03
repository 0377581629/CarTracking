using System.Collections.Generic;
using Abp.Application.Services.Dto;
using DPS.Cms.Application.Shared.Dto.Page;

namespace DPS.Cms.Application.Shared.Dto.PageLayout
{
	public class PageLayoutBlockDto : EntityDto<int?>
	{
		public string Code { get; set; }
		
		public string Name { get; set; }
		
		public string UniqueId { get; set; }
        
		public int PageLayoutId { get; set; }

		public int ColumnCount { get; set; }
        
		public bool WrapInRow { get; set; }
		
		public int Order { get; set; }

		public int? ParentBlockId { get; set; }

		public string ParentBlockUniqueId { get; set; }
		
		public string ParentColumnUniqueId { get; set; }

		#region Block Attribute
        
		public string Col1Id { get; set; }
        
		public string Col1UniqueId { get; set; }
        
		public string Col1Class { get; set; }
        
		public string Col2Id { get; set; }
        
		public string Col2UniqueId { get; set; }
        
		public string Col2Class { get; set; }
        
		public string Col3Id { get; set; }
        
		public string Col3UniqueId { get; set; }
        
		public string Col3Class { get; set; }
        
		public string Col4Id { get; set; }
        
		public string Col4UniqueId { get; set; }
        
		public string Col4Class { get; set; }
        
		#endregion
		
		public List<PageLayoutBlockDto> SubBlocks { get; set; }
	}
}