using Microsoft.Extensions.DependencyInjection;
using TaskManagerApiV2.Application.Services;
using TaskManagerApiV2.Domain.Abstractions;

namespace TaskManagerApiV2.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITaskService, TaskService>();
    }
}