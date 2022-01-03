using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace DPS.Cms.Core.Post
{
    [Table("Cms_Post_Category_Detail")]
    public class PostCategoryDetail : Entity
    {
        public int PostId { get; set; }
        
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }
        
        public int CategoryId { get; set; }
        
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}