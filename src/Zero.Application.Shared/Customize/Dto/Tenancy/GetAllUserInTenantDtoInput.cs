namespace Zero.Dto.Tenancy
{
    public class GetAllUserInTenantDtoInput : SelectByTenantDtoInput
    {
        public int OrganizationUnitId {get;set;}
        public bool ChildTenant { get; set; }
    }
}