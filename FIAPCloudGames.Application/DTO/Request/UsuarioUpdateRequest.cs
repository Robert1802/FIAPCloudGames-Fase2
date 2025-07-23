namespace FIAPCloudGames.Application.DTO.Request
{
    public record UsuarioUpdateRequest(
        int Id,
        string Nome,
        string Senha
    );
}
