using System.ComponentModel;
using Zero.Extensions;

namespace DPS.Lib.Application.Shared.Importing.Organization.WorkDepartment
{
    public class ImportWorkDepartmentDto
    {
        public long WorkDepartmentId { get; set; }

        [DisplayName("A")] 
        [InvalidExport] 
        public string WorkDepartmentCode { get; set; }

        [DisplayName("B")]
        [InvalidExport]
        public string WorkDepartmentName { get; set; }

        [DisplayName("C")]
        [InvalidExport]
        public string WorkDepartmentDescribe { get; set; }
        
        [InvalidExport]
        public string Exception { get; set; }
    }
}