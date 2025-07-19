﻿using FIAPCloudGames.Core.Entity;

namespace FIAPCloudGames.Core.Repository
{
    public interface IUsuarioJogoRepository : IRepository<UsuarioJogo>
    {
        UsuarioJogo? ObterPorIdUsuarioIdJogo(int idUsuario, int idJogo);

        List<UsuarioJogo> JogosCompradosPorUsuario(int idUsuario);
    }
}
