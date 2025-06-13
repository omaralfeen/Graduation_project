using System.ComponentModel.DataAnnotations;

namespace Graduation_project.DTO.Offer
{
    public class OfferUpdateDTO
    {
        [Required]
        [Range(1.0, double.MaxValue, ErrorMessage = "يجب أن يكون السعر أكبر من 0.")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "يجب ألا يتجاوز طول الرسالة 500 حرف.")]
        public string Message { get; set; }
    }
}
