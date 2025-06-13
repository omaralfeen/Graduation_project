//using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations;

//namespace Graduation_project.Models
//{
//    public class User
//    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int Id { get; set; }

//        [Required]
//        [StringLength(100, MinimumLength = 3)]
//        public string Name { get; set; }

//        [Required]
//        [DataType(DataType.Password)]
//        public string Password { get; set; }

//        [Required]
//        [RegularExpression("^(Client|Craftsman)$", ErrorMessage = "Role must be either 'Client' or 'Craftsman'.")]
//        public string UserType { get; set; }
//    }
//}
