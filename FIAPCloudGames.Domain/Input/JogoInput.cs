using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAPCloudGames.Domain.Input
{
    public class JogoInput
    {
        public string? Nome { get; set; }
        public string? Empresa { get; set; }
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
    }
}
