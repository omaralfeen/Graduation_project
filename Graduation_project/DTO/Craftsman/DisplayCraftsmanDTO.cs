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
        [StringLength(14, MinimumLength = 14, ErrorMessage = "الرقم القومي يجب أن يكون 14 رقماً.")]
        public string National_No { get; set; }
        //--
        [Required]
        public string Craft_Type { get; set; }
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح.")]
        public string Email { get; set; }
        [RegularExpression(@"^01[0-2,5]{1}[0-9]{8}$", ErrorMessage = "رقم الهاتف يجب أن يكون رقمًا مصريًا صالحًا.")]
        public string PhoneNumber { get; set; }
    }
}
