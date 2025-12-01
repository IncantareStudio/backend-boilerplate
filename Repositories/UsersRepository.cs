using test.Repositories.Interfaces;
using test.Models;
using Dapper;
using System.Data;

namespace test.Repositories;

public class UsersRepository : IUsersRepository
{
	private readonly IDbConnection _connection;

	public UsersRepository(IDbConnection connection) {
		_connection = connection;
	}

	public async Task<UsersModel?> GetByUsernameAsync(string username) {
		const string sql = "SELECT * FROM users WHERE username = @Username";
		return await _connection.QueryFirstOrDefaultAsync<UsersModel>(sql, new { Username = username });
	}

	public async Task<Guid> CreateAsync(UsersModel user) {
		const string sql = @"
			INSERT INTO users (username, password_hash)
			VALUES (@Username, @Password_Hash)
			RETURNING id;
		";

		return await _connection.ExecuteScalarAsync<Guid>(sql, user);
	}
}
