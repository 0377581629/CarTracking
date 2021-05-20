using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace Zero.Customize
{
    [Table("AbpEditionPermissions")]
    public class EditionPermission : FullAuditedEntity
    {
        public int EditionId { get; set; }
        
        public string PermissionName { get; set; }
    }
}