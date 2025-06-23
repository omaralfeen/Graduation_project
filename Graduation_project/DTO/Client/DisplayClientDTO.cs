using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_project.DTO.Client
{
    public class DisplayClientDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "العنوان مطلوب.")]
        [StringLength(200, ErrorMessage = "العنوان يجب ألا يتجاوز 200 حرف.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح.")]
        public string Email { get; set; }

        [RegularExpression(@"^01[0-2,5]{1}[0-9]{8}$", ErrorMessage = "رقم الهاتف غير صالح.")]
        public string PhoneNumber { get; set; }
        public string? ProfileImage { get; set; }
    }
}
