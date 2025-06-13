using System.ComponentModel.DataAnnotations;

namespace Graduation_project.DTO.Authentication
{
    public class RegisterCraftsmanDTO
    {
        [Required(ErrorMessage = "اسم المستخدم مطلوب.")]
        [StringLength(50)]
        public string UserName { get; set; }
        //--
        [Required(ErrorMessage = "الرقم القومي مطلوب.")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "الرقم القومي يجب أن يكون 14 رقماً.")]
        public string National_No { get; set; }
        //--
        [Required(ErrorMessage = "نوع الحرفة مطلوب.")]
        public string CraftType { get; set; }
        //--
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح.")]
        public string Email { get; set; }
        //--
        [Required(ErrorMessage = "كلمة المرور مطلوبة.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        //--
        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "كلمتا المرور غير متطابقتين.")]
        public string ConfirmPassword { get; set; }
        //--
        [RegularExpression(@"^01[0-2,5]{1}[0-9]{8}$", ErrorMessage = "رقم الهاتف يجب أن يكون رقمًا مصريًا صالحًا.")]
        public string PhoneNumber { get; set; }
    }

}
