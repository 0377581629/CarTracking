using System.ComponentModel.DataAnnotations;

namespace Zero.Web.Models.Account
{
    public class SendPasswordResetLinkViewModel
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}