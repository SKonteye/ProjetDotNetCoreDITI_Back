namespace ProjetDiti5.Model.Authentification
{
    public class TokenModel
    {
        public long Id { get; set; }

        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
