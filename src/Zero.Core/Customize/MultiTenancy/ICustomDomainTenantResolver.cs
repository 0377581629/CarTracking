namespace Zero.Customize.MultiTenancy
{
    public interface ICustomDomainTenantResolver
    {
        int? ResolveTenantId();
    }
}