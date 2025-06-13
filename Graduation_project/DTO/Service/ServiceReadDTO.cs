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
    }
}
