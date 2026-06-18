using Microsoft.AspNetCore.Identity;

namespace EventPortal.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
