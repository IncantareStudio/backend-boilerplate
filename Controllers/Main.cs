using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using test.Infrastructure.JWT;

namespace test.Controllers;

[ApiController]
[Route("")]
public class MainController : ControllerBase
{
	private readonly ITokenService _tokenService;

	public MainController(ITokenService tokenService) {
		_tokenService = tokenService;
	}

	[HttpGet]
	[SwaggerOperation(Summary = "Testa se a API está funcionando.")]
	public IActionResult HelloWorld() {
		return Ok("Hello, World!");
	}

	[HttpGet("/a")]
	[Authorize]
	public IActionResult Test() {
		return Ok("HI");
	}

	[HttpGet("/login")]
	public IActionResult Login() {
		return Ok(_tokenService.GenerateToken("69"));
	}
}
