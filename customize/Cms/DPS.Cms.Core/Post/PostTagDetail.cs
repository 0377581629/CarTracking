using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace DPS.Cms.Core.Post
{
    [Table("Cms_Post_Tag_Detail")]
    public class PostTagDetail : FullAuditedEntity
    {
        public int PostId { get; set; }
        
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }
        
        public int TagId { get; set; }
        
        [ForeignKey("TagId")]
        public virtual Tags Tag { get; set; }
    }
}