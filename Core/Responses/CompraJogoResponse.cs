﻿namespace FIAPCloudGames.Core.Responses
{
    public record CompraJogoResponse
    (
        int UsuarioId,
        int JogoId,
        string NomeJogo,
        decimal PrecoPago,
        int? PromocaoId,
        decimal? DescontoAplicado,
        string Mensagem
    );
}
