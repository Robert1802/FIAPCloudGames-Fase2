namespace FIAPCloudGames.Application.DTO.Request
{
    public class UsuarioUpdateRequest
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Senha { get; set; }
    }
}
