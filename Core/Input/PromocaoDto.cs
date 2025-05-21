using Core.Entity;

namespace Core.Input
{
    public class PromocaoDto
    {
        public string Nome { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public List<int> IdJogo { get; set; }
        public List<decimal> descontoJogo { get; set; }
    }
}
