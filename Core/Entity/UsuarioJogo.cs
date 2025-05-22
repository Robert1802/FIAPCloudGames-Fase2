using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Core.Entity
{
    public class UsuarioJogo : EntityBase
    {
        [Required]
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public virtual required Usuario Usuario { get; set; }

        [Required]
        [ForeignKey("Jogo")]
        public int JogoId { get; set; }
        public virtual required Jogo Jogo { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 0)")]
        public decimal PrecoDaCompra { get; set; }

        [ForeignKey("Promocao")]
        public int? PromocaoId { get; set; }
        public virtual Promocao? Promocao { get; set; }
    }
}
