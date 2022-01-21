using Newtonsoft.Json;

namespace AbstractApi.Models
{
    public class ValidEmailResult
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        
        [JsonProperty("autocorrect")]
        public string AutoCorrect { get; set; }
        
        [JsonProperty("deliverability")]
        public string DeliverAbility { get; set; }
        
        [JsonProperty("quality_score")]
        public double QualityScore { get; set; }
        
        [JsonProperty("is_valid_format")]
        public BooleanValueResponse Format { get; set; }

        public bool IsValidFormat => Format.Value;
        
        [JsonProperty("is_free_email")]
        public BooleanValueResponse FreeEmail { get; set; }

        public bool IsFreeEmail => FreeEmail.Value;
        
        [JsonProperty("is_disposable_email")]
        public BooleanValueResponse DisposableEmail { get; set; }

        public bool IsDisposableEmail => DisposableEmail.Value;
        
        [JsonProperty("is_role_email")]
        public BooleanValueResponse RoleEmail { get; set; }

        public bool IsRoleEmail => RoleEmail.Value;
        
        [JsonProperty("is_catchall_email")]
        public BooleanValueResponse CatchallEmail { get; set; }

        public bool IsCatchallEmail => CatchallEmail.Value;
        
        [JsonProperty("is_mx_found")]
        public BooleanValueResponse MxFound { get; set; }

        public bool IsMxFound => MxFound.Value;
        
        [JsonProperty("is_smtp_valid")]
        public BooleanValueResponse SmtpValid { get; set; }

        public bool IsSmtpValid => SmtpValid.Value;
    }
}