﻿using FIAPCloudGames.Domain.Input;
using FIAPCloudGames.Domain.Repository;
using FIAPCloudGames.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FIAPCloudGames.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUsuarioRepository _repo;
    private readonly TokenService _tokenService;

    public AuthController(IUsuarioRepository repo, TokenService tokenService)
    {
        _repo = repo;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginInput input)
    {
        var usuario = _repo.Logar(input.Email!, input.Senha!);
        if (usuario == null)
            return Unauthorized("Usuário ou senha inválidos");

        var token = _tokenService.GerarToken(usuario);
        return Ok(new { token });
    }
}
