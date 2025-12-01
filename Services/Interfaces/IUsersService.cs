using test.DTOs;

namespace test.Services.Interfaces;

public interface IUsersService
{
	Task<string?> Authentication(LoginDTO loginDto);
	Task<Guid> CreateUserAsync(LoginDTO dto);
}
