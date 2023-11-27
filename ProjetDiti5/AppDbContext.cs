using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjetDiti5.Model;
using ProjetDiti5.Model.Authentification;

namespace ProjetDiti5
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Article> Articles { get; set; }
        public DbSet<LoginModel> LoginModels { get; set; }
        public DbSet<RegisterModel> RegisterModels { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<TokenModel> tokenModels { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
    }
}
