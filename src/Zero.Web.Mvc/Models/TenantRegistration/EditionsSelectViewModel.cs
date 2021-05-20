using Abp.AutoMapper;
using Zero.MultiTenancy.Dto;

namespace Zero.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(EditionsSelectOutput))]
    public class EditionsSelectViewModel : EditionsSelectOutput
    {
    }
}
