using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManagerApiV2.Application.Abstractions;
using TaskManagerApiV2.Persistence.Repositories;

namespace TaskManagerApiV2.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDatabase(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("TaskManagerApiV2Db"));
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}