using System;
using Abp;

namespace Zero.Importing
{
    public class ImportFromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }
        
        public string Lang { get; set; }
    }
}