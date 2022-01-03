namespace Zero.Net.Emailing
{
    public interface IEmailTemplateProvider
    {
        string GetDefaultTemplate(int? tenantId, ZeroEnums.EmailTemplateType? emailTemplateType = null);
    }
}
