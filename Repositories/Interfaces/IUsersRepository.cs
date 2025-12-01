using test.Models;

namespace test.Repositories.Interfaces;

public interface IUsersRepository
{
	Task<UsersModel?> GetByUsernameAsync(string username);
	Task<Guid> CreateAsync(UsersModel user);
}
