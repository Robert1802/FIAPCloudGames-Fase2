﻿using FIAPCloudGames.Application.DTO;

namespace FIAPCloudGames.Application.DTO
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
        public string? NivelAcesso { get; set; }
        public decimal? Saldo { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
