using System.ComponentModel.DataAnnotations;

namespace Zero.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
