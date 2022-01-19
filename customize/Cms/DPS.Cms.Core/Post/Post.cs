using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using DPS.Cms.Core.Shared;
using Zero.Customize.Base;

namespace DPS.Cms.Core.Post
{
    [Table("Cms_Post")]
    public class Post : SimpleFullAuditedEntity, IHaveSeoInfo, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        
        public int CategoryId { get; set; }
        
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public string About { get; set; }

        public string Summary { get; set; }
        
        public string Slug { get; set; }
        
        public string Url { get; set; }
        
        public string Image { get; set; }

        public int CommentCount { get; set; }
        
        public int ViewCount { get; set; }

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