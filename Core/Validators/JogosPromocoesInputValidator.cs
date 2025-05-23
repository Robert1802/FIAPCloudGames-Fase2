using Core.Input;
using FluentValidation;

namespace Core.Validators
{
    public class JogosPromocoesInputValidator : AbstractValidator<JogosPromocoesInput>
    {
        public JogosPromocoesInputValidator()
        {
            RuleFor(x => x.JogoId)
                .GreaterThan(0)
                .WithMessage("O Id do jogo deve ser maior que 0.");

            RuleFor(x => x.PromocaoId)
                .GreaterThan(0)
                .WithMessage("O Id da promoção deve ser maior que 0.");

            RuleFor(x => x.Desconto)
                .GreaterThan(0)
                .WithMessage("O desconto deve ser maior que 0.");
        }
    }
}