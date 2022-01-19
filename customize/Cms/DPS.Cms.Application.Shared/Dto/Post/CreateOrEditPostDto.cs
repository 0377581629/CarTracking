using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Abp.Domain.Entities;
using DPS.Cms.Application.Shared.Dto.Post.PostCategory;
using DPS.Cms.Application.Shared.Dto.Post.PostTags;
using DPS.Cms.Application.Shared.Dto.Tags;
using DPS.Cms.Core.Post;
using DPS.Cms.Core.Shared;
using JetBrains.Annotations;
using Zero;
using Zero.Customize.Dto.Base;

namespace DPS.Cms.Application.Shared.Dto.Post
{
    public class CreateOrEditPostDto  : SimpleCreateOrEditDto, IHaveSeoInfo, IMayHaveTenant
    {
        public int? CategoryId { get; set; }
        
        public string CategoryCode { get; set; }
        
        public string CategoryName { get; set; }

        public string About { get; set; }

        public string Summary { get; set; }
        
        public string Slug { get; set; }
        
        public string Url { get; set; }
        
        public string Image { get; set; }

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

        public int? TenantId { get; set; }
        
        [CanBeNull] 
        public List<PostTagDetailDto> ListTags { get; set; }
        
        
        public List<PostCategoryDetailDto> ListCategories { get; set; }
        
        // public List<PostTagDetailDto> ListTags { get; set; }
    
        [StringLength(ZeroConst.MaxNameLength, MinimumLength = ZeroConst.MinNameLength)]
        public string Name { get; set; }
    }
}