using Graduation_project.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Graduation_project.DTO.Offer
{
    public class OfferCreateDTO
    {
        [Required]
        [Range(1.0, double.MaxValue, ErrorMessage = "يجب أن يكون السعر أكبر من 0.")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "يجب ألا يتجاوز طول الرسالة 500 حرف.")]
        public string Message { get; set; }

        [Required]
        public int ServiceId { get; set; }
        //--
        [Required]
        public int CraftsmanId { get; set; }

    }
}
