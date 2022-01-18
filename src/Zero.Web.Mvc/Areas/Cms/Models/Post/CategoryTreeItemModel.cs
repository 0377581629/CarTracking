using System.Collections.Generic;
using Abp.Application.Services.Dto;
using DPS.Cms.Application.Shared.Dto.Category;
using JetBrains.Annotations;

namespace Zero.Web.Areas.Cms.Models.Post
{
    public class CategoryTreeItemModel
    {
        public ListResultDto<CategoryDto> Categories { get; set; }

        public int? ParentId { get; set; }

        public List<int> ListParentSelected { get; set; }

        public CategoryTreeItemModel()
        {
        }

        public CategoryTreeItemModel(ListResultDto<CategoryDto> categories, int? parentId, [CanBeNull] List<int> listParentSelected)
        {
            Categories = categories;
            ParentId = parentId;
            ListParentSelected = listParentSelected;
        }
    }
}