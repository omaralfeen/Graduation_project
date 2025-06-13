using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_project.Models
{
    public class Craftsman //:User 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //--
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }
        //--
        [Required]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "الرقم القومي يجب أن يكون 14 رقماً.")]
        public string National_No { get; set; }
        //--
        [Required]
        public string Craft_Type { get; set; }
        //--

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        //
        public virtual List<Offer> Offers { get; set; } = new List<Offer>();
    }
}
