using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Graduation_project.Models
{
    public class Offer
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
        //--
        [Required]
        public bool IsAccepted { get; set; } = false;
        //--
        [Required]
        [ForeignKey("Service")]
        public int ServiceId { get; set; }
        public virtual Service Service { get; set; }
        //--
        [Required]
        [ForeignKey("Craftsman")]
        public int CraftsmanId { get; set; }
        public virtual Craftsman Craftsman { get; set; }
    }
}
