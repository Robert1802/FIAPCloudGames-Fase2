using Core.Entity;

namespace Core.Input
{
    public class JogoDescontoInput
    {
        public int Id { get; set; }
        public required string Nome { get; set; }    
        public required decimal ValorDesconto { get; set; }
        public required int IdPromocao { get; set; }
        
    }
}
