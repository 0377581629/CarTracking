using Abp.AutoMapper;
using DPS.Cms.Core.Shared;
using Zero.Customize.Dto.Base;

namespace DPS.Cms.Application.Shared.Dto.Post
{
    [AutoMapFrom(typeof(Core.Post.Post))]
    public class PostDto : SimpleEntityDto, IHaveSeoInfo
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
    }
}