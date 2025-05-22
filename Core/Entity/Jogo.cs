using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Core.Entity
{
    public class Jogo : EntityBase
    {
        [Required]
        [MaxLength(200)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(400)]
        public string Descricao { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Preco { get; set; }
                
        [Column(TypeName = "decimal(3, 2)")]
        public decimal Desconto { get; set; }

        public string? Empresa { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<UsuarioJogo> UsuarioJogos { get; set; }
        public virtual ICollection<JogosPromocoes> JogosPromocoes { get; set; }

    }
}
