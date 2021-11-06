namespace Zero.Authorization.Accounts.Dto
{
    public class RegisterOutput
    {
        public bool IsSuccess { get; set; }
        
        public bool IsEmailConfirmationRequiredForLogin { get; set; }
        public bool CanLogin { get; set; }
    }
}