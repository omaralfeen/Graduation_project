using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_project.Models
{
    public class Client//:User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //--
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }
        //--
        [Required(ErrorMessage = "العنوان مطلوب.")]
        [StringLength(200, ErrorMessage = "العنوان يجب ألا يتجاوز 200 حرف.")]
        public string Address { get; set; }

        //-----
        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        //--
        public virtual List<Service> Services { get; set; }
    }
}
