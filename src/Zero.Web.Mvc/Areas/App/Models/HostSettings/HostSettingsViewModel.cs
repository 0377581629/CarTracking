using System.Collections.Generic;
using System.Linq;
using Abp.Application.Services.Dto;
using Zero.Configuration.Host.Dto;
using Zero.Editions.Dto;
using Zero.MultiTenancy.Payments;

namespace Zero.Web.Areas.App.Models.HostSettings
{
    public class HostSettingsViewModel
    {
        public HostSettingsEditDto Settings { get; set; }

        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public List<ComboboxItemDto> TimezoneItems { get; set; }

        public List<string> EnabledSocialLoginSettings { get; set; } = new();
        
        public List<PaymentGatewayModel> ActivePaymentGateways { get; set; }

        public bool ActivePayment => ActivePaymentGateways != null && ActivePaymentGateways.Any();
    }
}