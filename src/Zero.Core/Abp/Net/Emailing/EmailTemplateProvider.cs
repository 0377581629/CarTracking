using System;
using System.Collections.Concurrent;
using System.Text;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.MultiTenancy;
using Abp.Reflection.Extensions;
using Zero.Customize;
using Zero.MultiTenancy;
using Zero.Url;

namespace Zero.Net.Emailing
{
    public class EmailTemplateProvider : IEmailTemplateProvider, ISingletonDependency
    {
        private readonly IWebUrlService _webUrlService;
        private readonly ITenantCache _tenantCache;
        private readonly ConcurrentDictionary<string, string> _defaultTemplates;
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;

        public EmailTemplateProvider(IWebUrlService webUrlService, ITenantCache tenantCache, IRepository<EmailTemplate> emailTemplateRepository)
        {
            _webUrlService = webUrlService;
            _tenantCache = tenantCache;
            _emailTemplateRepository = emailTemplateRepository;
            _defaultTemplates = new ConcurrentDictionary<string, string>();
        }

        public string GetDefaultTemplate(int? tenantId, ZeroEnums.EmailTemplateType? emailTemplateType = null)
        {
            var res = "";
            var tenancyKey = tenantId.HasValue ? tenantId.Value.ToString() : "host";
            res = _defaultTemplates.GetOrAdd(tenancyKey, key =>
            {
                using var stream = typeof(EmailTemplateProvider).GetAssembly().GetManifestResourceStream("Zero.Abp.Net.Emailing.EmailTemplates.default.html");
                var bytes = stream.GetAllBytes();
                var template = Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);
                return template;
            });

            if (emailTemplateType.HasValue)
            {
                var emailTemplate = _emailTemplateRepository.FirstOrDefault(o => o.TenantId == tenantId && o.IsActive && o.EmailTemplateType == (int)emailTemplateType.Value);
                if (emailTemplate != null)
                    res = emailTemplate.Content;    
            }
            else
            {
                var emailTemplate = _emailTemplateRepository.FirstOrDefault(o => o.TenantId == tenantId && o.IsActive && o.EmailTemplateType == null);
                if (emailTemplate != null)
                    res = emailTemplate.Content;
            }
            
            // Replace base info and return

            return res.Replace("{THIS_YEAR}", DateTime.Now.Year.ToString())
                .Replace("{EMAIL_LOGO_URL}", GetTenantLogoUrl(tenantId));
        }

        private string GetTenantLogoUrl(int? tenantId)
        {
            if (!tenantId.HasValue)
            {
                return _webUrlService.GetServerRootAddress().EnsureEndsWith('/') + "TenantCustomization/GetTenantLogo?skin=light";
            }

            var tenant = _tenantCache.Get(tenantId.Value);
            return _webUrlService.GetServerRootAddress(tenant.TenancyName).EnsureEndsWith('/') + "TenantCustomization/GetTenantLogo?skin=light&tenantId=" + tenantId.Value;
        }
    }
}