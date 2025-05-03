using TaskManagerApiV2.Domain.Entities;

namespace TaskManagerApiV2.Domain.Abstractions;

public interface IUserService
{
    Task<User> CreateAsync(string username);
    Task<List<User>> GetAllAsync();
}