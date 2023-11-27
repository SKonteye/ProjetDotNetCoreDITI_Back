using System.ComponentModel.DataAnnotations;

namespace ProjetDiti5.Model.Authentification
{
    public class LoginModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }

}
