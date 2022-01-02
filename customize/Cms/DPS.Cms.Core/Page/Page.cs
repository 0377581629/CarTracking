using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Zero.Customize.Base;

namespace DPS.Cms.Core.Page
{
    [Table("Cms_Page")]
    public class Page : SimpleFullAuditedEntity, IMayHaveTenant
    {
        #region Properties
        public int? TenantId { get; set; }
        
        public int PageLayoutId { get; set; }
        
        [ForeignKey("PageLayoutId")]
        public virtual PageLayout PageLayout { get; set; }
        
        public string Summary { get; set; }
        
        public string Content { get; set; }
        
        public string About { get; set; }
        
        public string Url { get; set; }
        
        public string Slug { get; set; }
        
        public bool IsHomePage { get; set; }

        public bool Publish { get; set; }

        #endregion
        
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