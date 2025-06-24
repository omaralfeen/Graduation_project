using Graduation_project.Enums;
using System.ComponentModel.DataAnnotations;

namespace Graduation_project.DTO.Craftsman
{
    public class DisplayCraftsmanDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }      
        //--
        [Required]
        public CraftType Craft_Type { get; set; }
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح.")]
        public string Email { get; set; }
        [RegularExpression(@"^01[0-2,5]{1}[0-9]{8}$", ErrorMessage = "رقم الهاتف يجب أن يكون رقمًا مصريًا صالحًا.")]
        public string PhoneNumber { get; set; }
        public string? ProfileImage { get; set; }
    }
}
