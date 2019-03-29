using System.ComponentModel.DataAnnotations;

namespace HC.AbpCore.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}