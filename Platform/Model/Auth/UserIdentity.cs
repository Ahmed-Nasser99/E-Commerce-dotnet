using Microsoft.AspNetCore.Identity;

namespace Platform.Model.Auth
{
    public class UserIdentity : IdentityUser
    {
        public string resetCode { get; set; }
    }
}
