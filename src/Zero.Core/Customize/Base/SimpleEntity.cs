using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Zero.Customize.Base
{
    public class SimpleEntity : Entity
    {
        public string Numbering { get; set; }
        
        [StringLength(ZeroConst.MaxCodeLength)]
        public string Code { get; set; }

        [Required]
        [StringLength(ZeroConst.MaxNameLength, MinimumLength = ZeroConst.MinNameLength)]
        public string Name { get; set; }

        public string Note { get; set; }
        
        public int Order { get; set; }
        
        public bool IsDefault { get; set; }
        
        public bool IsActive { get; set; }
    }
}