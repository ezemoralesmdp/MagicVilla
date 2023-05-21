using Microsoft.AspNetCore.Identity;

namespace MagicVilla_API.Models
{
    public class UserApplication : IdentityUser
    {
        public string Name { get; set; }
    }
}