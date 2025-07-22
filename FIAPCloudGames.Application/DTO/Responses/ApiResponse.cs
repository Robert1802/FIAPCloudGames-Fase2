namespace FIAPCloudGames.Domain.Responses
{
    public record class ApiResponse<T>
    {
        public bool Sucesso { get; init; }
        public T? Dados { get; init; }
        public ErroResponse? Erro { get; init; }

        public static ApiResponse<T> Ok(T dados)
            => new() { Sucesso = true, Dados = dados };

        public static ApiResponse<T> Falha(int statusCode, string mensagem)
            => new() { Sucesso = false, Erro = new ErroResponse(statusCode, mensagem) };
    }
}
