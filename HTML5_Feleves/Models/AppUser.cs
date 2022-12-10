using Microsoft.AspNetCore.Identity;

namespace HTML5_Feleves.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
    }
}
