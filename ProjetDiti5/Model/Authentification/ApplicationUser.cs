using Microsoft.AspNetCore.Identity;

namespace ProjetDiti5.Model.Authentification
{
    public class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
