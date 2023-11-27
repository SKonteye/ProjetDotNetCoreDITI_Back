using System.ComponentModel.DataAnnotations;

namespace ProjetDiti5.Model
{
    public class Article
    {
        [Key]
        public int IdArticle { get; set; }
        [Required]
        public int CodeArticle { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public int Quantite { get; set; }
        public int? QuantiteMin { get; set; }


        public double PrixUnit { get; set; }

        public DateTime DatePeremption { get; set; }
    }
}
