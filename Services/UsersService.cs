using test.DTOs;
using test.Models;
using test.Repositories.Interfaces;
using test.Services.Interfaces;
using test.Infrastructure.JWT;

namespace test.Services;

public class UsersService : IUsersService
{
	private readonly IUsersRepository _userRepository;
	private readonly ITokenService _tokenService;

	public UsersService(IUsersRepository repository, ITokenService tokenService) {
		_userRepository = repository;
		_tokenService = tokenService;
	}

	public async Task<string?> Authentication(LoginDTO loginDto) {
		var user = await _userRepository.GetByUsernameAsync(loginDto.Username);
		if (user == null) return null;

		if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password_Hash))
			return null;

		return _tokenService.GenerateToken(loginDto.Password);
	}

	public async Task<Guid> CreateUserAsync(LoginDTO dto) {
		var hashPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

		var user = new UsersModel {
			Username = dto.Username,
			Password_Hash = hashPassword
		};

		return await _userRepository.CreateAsync(user);
	}
}
