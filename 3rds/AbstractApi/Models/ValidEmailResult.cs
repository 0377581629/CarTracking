namespace AbstractApi.Models
{
    public class ValidEmailResult
    {
        public string Email { get; set; }
        
        public string AutoCorrect { get; set; }
        
        public string DeliverAbility { get; set; }
        
        public double QualityScore { get; set; }
        
        public bool IsValidFormat { get; set; }
        
        public bool IsFreeEmail { get; set; }
        
        public bool IsDisposableEmail { get; set; }
        
        public bool IsRoleEmail { get; set; }
        
        public bool IsCatchallEmail { get; set; }
        
        public bool IsMxFound { get; set; }
        
        public bool IsSmtpValid { get; set; }
    }
}