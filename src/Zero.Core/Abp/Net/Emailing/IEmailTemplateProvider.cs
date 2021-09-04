﻿namespace Zero.Net.Emailing
{
    public interface IEmailTemplateProvider
    {
        string GetDefaultTemplate(int? tenantId, ZEROEnums.EmailTemplateType? emailTemplateType = null);
    }
}
