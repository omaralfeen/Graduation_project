using System.ComponentModel.DataAnnotations;

namespace Graduation_project.DTO.Authentication
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح.")]
        public string Email { get; set; }
        //---
        [Required(ErrorMessage = "كلمة المرور مطلوبة.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
