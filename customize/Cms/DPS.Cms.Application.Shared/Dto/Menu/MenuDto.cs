using System.Collections.Generic;
using Zero.Customize.Dto.Base;

namespace DPS.Cms.Application.Shared.Dto.Menu
{
    public class MenuDto: SimpleEntityDto
    {
        public int? TenantId { get; set; }
        
        public long? ParentId { get; set; }
        public int MenuGroupId { get; set; }
        
        public string MenuGroupCode { get; set; }
		
        public string MenuGroupName { get; set; }
        
        public string Url { get; set; }
    }
}