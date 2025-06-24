using Graduation_project.Enums;
using System.ComponentModel.DataAnnotations;

namespace Graduation_project.DTO.Service
{
    public class ServiceReadDTO
    {
        [Key]
        public int Id { get; set; }
        //--
        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "يجب أن يكون العنوان بين 5 و 50 حرف.")]
        public string Title { get; set; }
        //--
        [Required]
        [StringLength(500, ErrorMessage = "يجب ألا يتجاوز الوصف 500 حرف.")]
        public string Description { get; set; }
        public string Type { get; set; }
        //--
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "يجب أن يكون الميزانية قيمة موجبة.")]
        public decimal Budget { get; set; }
        //--

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        //--
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string ClientName { get; set; }

        [Required(ErrorMessage = "العنوان مطلوب.")]
        [StringLength(200, ErrorMessage = "العنوان يجب ألا يتجاوز 200 حرف.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح.")]
        public string Email { get; set; }

        [RegularExpression(@"^01[0-2,5]{1}[0-9]{8}$", ErrorMessage = "رقم الهاتف غير صالح.")]
        public string PhoneNumber { get; set; }

        public string? ImageUrl { get; set; }

    }
}
