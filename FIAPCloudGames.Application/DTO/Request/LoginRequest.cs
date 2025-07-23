namespace FIAPCloudGames.Application.DTO.Request
{
    public record LoginRequest(
        string Email,
        string Senha
    );
}
