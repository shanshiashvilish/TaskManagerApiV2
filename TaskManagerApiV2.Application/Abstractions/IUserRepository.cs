using TaskManagerApiV2.Domain.Entities;

namespace TaskManagerApiV2.Application.Abstractions;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default);
}