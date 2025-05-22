using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    public class Promocao : EntityBase
    {
       
        [Required]
        [MaxLength(50)]
        public required string Nome { get; set; }

        [Required]
        public DateTime DataInicio { get; set; }

        [Required]
        public DateTime DataFim { get; set; }

        [Required]
        public bool Ativo { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public virtual required Usuario Usuario { get; set; }

        public virtual ICollection<JogosPromocoes> JogosPromocoes { get; set; }
        public virtual ICollection<UsuarioJogo> UsuarioJogos { get; set; }
    }
}
