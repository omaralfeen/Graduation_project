using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Graduation_project.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "يجب أن يكون العنوان بين 5 و 50 حرف.")]
        public string Title { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "يجب ألا يتجاوز الوصف 500 حرف.")]
        public string Description { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "يجب أن يكون الميزانية قيمة موجبة.")]
        public decimal Budget { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? ImageUrl {  get; set; }
        //--
        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
        //--
        public virtual List<Offer> Offers { get; set; } = new List<Offer>();
    }
}
