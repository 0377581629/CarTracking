using System.Collections.Generic;
using Abp.Application.Services.Dto;
using DPS.Cms.Application.Shared.Dto.Category;
using DPS.Cms.Application.Shared.Dto.Post;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Zero.Web.Areas.Cms.Models.Post
{
    public class CreateOrEditPostViewModel
    {
        public List<SelectListItem> ListCategory { get; set; }
        
        public ListResultDto<CategoryDto> Categories { get; set; }
        public CreateOrEditPostDto Post { get; set; }
        public bool IsEditMode => Post.Id.HasValue;
    }
}