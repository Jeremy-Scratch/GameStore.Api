using Dapper;
using GameStore.Api.Entities;
using System.Data;

namespace GameStore.Api.Repositories;

public class UsersRepo : IUsersRepo
{
    private readonly IDbConnection _connection;
    public UsersRepo(IDbConnection connection)
    {
        _connection = connection;
        _connection.Open();
    }
    public async Task<Users?> GetById(int id)
    {
        var sql = "SELECT * FROM users WHERE id = @Id";
        return await _connection.QuerySingleOrDefaultAsync<Users>(sql, new { Id = id });
    }
    public async Task<Users?> CheckEmail(string email)
    {
        var sql = "SELECT * FROM users WHERE email = @Email";
        return await _connection.QuerySingleOrDefaultAsync<Users>(sql, new { Email = email });
    }
    public async Task<int> Create(Users user)
    {
        var sql = "INSERT INTO users(name,email,\"passwordHash\",role) VALUES (@Name, @Email, @PasswordHash, @Role) RETURNING id";
        var newId = await _connection.ExecuteScalarAsync<int>(sql, new
        {
            user.Name,
            user.Email,
            user.PasswordHash,
            user.Role
        });
        return newId;
    }
}