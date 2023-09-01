using System.ComponentModel.DataAnnotations;

namespace AFSTranslate.ViewModels
{
    public class SignUpViewModel
    {
        public string OtherName { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
