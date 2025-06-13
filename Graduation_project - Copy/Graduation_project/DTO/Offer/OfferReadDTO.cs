using System.ComponentModel.DataAnnotations;

namespace Graduation_project.DTO.Offer
{
    public class OfferReadDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1.0, double.MaxValue, ErrorMessage = "يجب أن يكون السعر أكبر من 0.")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "يجب ألا يتجاوز طول الرسالة 500 حرف.")]
        public string Message { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public bool IsAccepted { get; set; } = false;

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string CraftsmanName { get; set; }
    }
}
