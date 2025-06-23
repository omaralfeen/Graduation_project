using Microsoft.AspNetCore.Identity;

namespace Graduation_project.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string? ProfileImage {  get; set; }
    }
}
