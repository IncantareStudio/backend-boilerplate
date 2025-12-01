namespace test.Models;

public class UsersModel
{
	public Guid Id { get; set; }
	public string Username { get; set; } = string.Empty;
	public string Password_Hash { get; set; } = string.Empty;
}
