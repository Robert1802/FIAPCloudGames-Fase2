using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FIAPCloudGames.Domain.Entity
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

        [JsonIgnore]
        public virtual Usuario? Usuario { get; set; }

        [JsonIgnore]
        public virtual ICollection<JogosPromocoes>? JogosPromocoes { get; set; }

        [JsonIgnore]
        public virtual ICollection<UsuarioJogo>? UsuarioJogos { get; set; }
    }
}
