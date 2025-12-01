using test.DTOs;
using test.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace test.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly IUsersService _service;

	public AuthController(IUsersService service) {
		_service = service;
	}

	[HttpPost("login")]
	[SwaggerOperation(Summary = "Autenticação do usuário", Description = "Envia usuário e senha. Retorna JWT válido por 1 hora.")]
	public async Task<IActionResult> Login(LoginDTO dto) {
		var token = await _service.Authentication(dto);
		if (token == null) return Unauthorized("Usuário ou senha inválidos.");

		return Ok(new { token });
	}

	[HttpPost("register")]
	[SwaggerOperation(Summary = "Registrar novo usuário.")]
	public async Task<IActionResult> Register(LoginDTO dto) {
		var id = await _service.CreateUserAsync(dto);

		return CreatedAtAction(nameof(Register), new { id }, new { id });
	}
}
