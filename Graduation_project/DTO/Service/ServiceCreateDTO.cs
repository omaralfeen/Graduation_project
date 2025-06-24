using Graduation_project.Enums;
using System.ComponentModel.DataAnnotations;

namespace Graduation_project.DTO.Service
{
    public class ServiceCreateDTO
    {
        //--
        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "يجب أن يكون العنوان بين 5 و 50 حرف.")]
        public string Title { get; set; }
        //--
        [Required]
        [StringLength(500, ErrorMessage = "يجب ألا يتجاوز الوصف 500 حرف.")]
        public string Description { get; set; }
        public ServiceType Type { get; set; }
        //--
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "يجب أن يكون الميزانية قيمة موجبة.")]
        public decimal Budget { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
