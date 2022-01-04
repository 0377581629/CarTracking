using System.Collections.Generic;
using DPS.Cms.Application.Shared.Dto.Category;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Zero.Web.Areas.Cms.Models.Category
{
    public class CategoryViewModel
    {
		public List<SelectListItem> ListWorkGroup { get; set; }
        
        public List<CategoryDto> Categories { get; set; }
    }
}