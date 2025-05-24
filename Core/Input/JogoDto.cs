using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Input
{
    public class JogoDto
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Empresa { get; set; }
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }        
        public DateTime DataCriacao { get; set; }
    }
}
