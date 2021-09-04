using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Zero.Customize
{
    [Table("AbpEmailTemplates")]
    public class EmailTemplate : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        
        public int? EmailTemplateType { get; set; }
        
        public string Title { get; set; }
        
        public string Content { get; set; }
        
        public string Sign { get; set; }
        
        public string Note { get; set; }
        
        public bool AutoCreateForNewTenant { get; set; }

        public bool IsActive { get; set; }
    }
}