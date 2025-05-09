using Microsoft.EntityFrameworkCore;
using TaskManagerApiV2.Application.Abstractions;
using TaskManagerApiV2.Domain.Entities;

namespace TaskManagerApiV2.Persistence.Repositories;

public class UserRepository(AppDbContext dbContext) : RepositoryBase<User>(dbContext), IUserRepository
{
    private readonly AppDbContext _db = dbContext;

    public async Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _db.Users.AnyAsync(u => u.Name == name, cancellationToken);
    }

    public override async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Users.Include(u => u.Tasks).ToListAsync(cancellationToken);
    }
}