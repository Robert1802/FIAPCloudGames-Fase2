using FIAPCloudGames.Application.DTO.Request;
using FluentValidation;

namespace FIAPCloudGames.Application.Validators
{
    public class JogosPromocoesRequestValidator : AbstractValidator<JogosPromocoesRequest>
    {
        public JogosPromocoesRequestValidator()
        {
            RuleFor(x => x.JogoId)
                .GreaterThan(0)
                .WithMessage("O Id do jogo deve ser maior que 0.");

            RuleFor(x => x.PromocaoId)
                .GreaterThan(0)
                .WithMessage("O Id da promoção deve ser maior que 0.");

            RuleFor(x => x.Desconto)
                .GreaterThan(0)
                .WithMessage("O desconto deve ser maior que 0.")
                .LessThanOrEqualTo(100)
                .WithMessage("O desconto deve ser menor ou igual a 100.");
        }
    }
}