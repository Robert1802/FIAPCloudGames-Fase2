using System.ComponentModel.DataAnnotations;

namespace FIAPCloudGames.Core.Entity
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime DataCriacao { get; set; }
    }
}
