﻿namespace FIAPCloudGames.Domain.Responses;

public class ErroResposta
{
    public int StatusCode { get; set; }
    public string Mensagem { get; set; } = string.Empty;
}
