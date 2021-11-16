namespace Zero.Abp.Configuration.Dto
{
    public class PaymentManagementSettingsEditDto
    {
        public bool AllowTenantUseCustomConfig { get; set; }
        public bool UseCustomPaymentConfig { get; set; }

        // Paypal
        
        public bool PayPalIsActive { get; set; }
        
        public string PayPalEnvironment { get; set; }

        public string PayPalClientId { get; set; }

        public string PayPalClientSecret { get; set; }

        public string PayPalDemoUsername { get; set; }
        
        public string PayPalDemoPassword { get; set; }
        
        // AlePay

        public bool AlePayIsActive { get; set; }

        public string AlePayBaseUrl { get; set; }

        public string AlePayTokenKey { get; set; }
        
        public string AlePayChecksumKey { get; set; }
    }
}