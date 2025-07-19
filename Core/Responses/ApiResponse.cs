namespace FIAPCloudGames.Core.Responses;

public class ApiResponse<T>
{
    public bool Sucesso { get; set; }
    public T? Dados { get; set; }
    public ErroResposta? Erro { get; set; }

    public static ApiResponse<T> Ok(T dados)
        => new() { Sucesso = true, Dados = dados };

    public static ApiResponse<T> Falha(int statusCode, string mensagem)
        => new() { Sucesso = false, Erro = new ErroResposta { StatusCode = statusCode, Mensagem = mensagem } };
}
