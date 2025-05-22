using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Core.Entity
{
    public class Usuario : EntityBase
    {
        [Required]
        [MaxLength(200)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string Senha { get; set; }

        [Required]
        [MaxLength(100)]
        public required string NivelAcesso { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Saldo { get; set; }

        // Relacionamentos
        public virtual ICollection<UsuarioJogo> UsuarioJogos { get; set; }
        public virtual ICollection<Promocao> Promocoes { get; set; }
        public virtual ICollection<JogosPromocoes> JogosPromocoes { get; set; }
        public virtual ICollection<Jogo> Jogos { get; set; }
    }
}
