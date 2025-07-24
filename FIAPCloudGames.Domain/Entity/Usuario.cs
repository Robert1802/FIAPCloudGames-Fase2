using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FIAPCloudGames.Domain.Entity
{
    public class Usuario : EntityBase
    {
        [Required]
        [MaxLength(200)]
        public string? Nome { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Senha { get; set; }

        [Required]
        [MaxLength(100)]
        public required string NivelAcesso { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Saldo { get; set; }

        // Relacionamentos
        [JsonIgnore]
        public virtual ICollection<UsuarioJogo>? UsuarioJogos { get; set; }
        [JsonIgnore]
        public virtual ICollection<Promocao>? Promocoes { get; set; }
        [JsonIgnore]
        public virtual ICollection<JogosPromocoes>? JogosPromocoes { get; set; }
        [JsonIgnore]
        public virtual ICollection<Jogo>? Jogos { get; set; }
    }
}
