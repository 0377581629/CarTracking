namespace Zero.Dto.Tenancy
{
    public class SelectByTenantDtoInput
    {
        public string Filter { get; set; }
        
        public int TenantId { get; set; }
    }
}