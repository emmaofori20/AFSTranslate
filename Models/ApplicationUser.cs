
using Microsoft.AspNetCore.Identity;

namespace AFSTranslate.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Othername { get; set; }
        public string? Surname { get; set; }

        public ICollection<Response> Responses { get; set; }

    }
}
