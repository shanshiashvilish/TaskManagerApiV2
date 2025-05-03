using Microsoft.Extensions.Logging;
using TaskManagerApiV2.Application.Abstractions;
using TaskManagerApiV2.Domain.Abstractions;
using TaskManagerApiV2.Domain.Entities;

namespace TaskManagerApiV2.Application.Services;

public class UserService(IUserRepository userRepository, ILogger<UserService> logger) : IUserService
{
    public async Task<User> CreateAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Name is required!");

        if (await userRepository.NameExistsAsync(username))
            throw new InvalidOperationException("User with this name already exists!");

        var user = new User { Name = username };

        await userRepository.AddAsync(user);
        await userRepository.SaveChangesAsync();

        logger.LogInformation("User '{UserName}' created with ID: {UserId}", user.Name, user.Id);

        return user;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await userRepository.GetAllAsync();
    }
}