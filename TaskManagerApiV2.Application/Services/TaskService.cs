using Microsoft.Extensions.Logging;
using TaskManagerApiV2.Application.Abstractions;
using TaskManagerApiV2.Domain.Abstractions;
using TaskManagerApiV2.Domain.Entities;
using TaskManagerApiV2.Domain.Enums;

namespace TaskManagerApiV2.Application.Services;

public class TaskService(ITaskRepository taskRepository, IUserService userService, ILogger<TaskService> logger)
    : ITaskService
{
    public async Task<TaskItem> CreateAsync(string title)
    {
        await ValidateTitle(title);

        var allUsers = await userService.GetAllAsync();

        var task = new TaskItem
        {
            Title = title,
            State = TaskState.Waiting
        };

        if (allUsers.Count > 0)
        {
            var chosenUserId = await GetUserIdForNewTaskAsync(allUsers);

            if (chosenUserId != Guid.Empty)
            {
                await AssignTask(task, chosenUserId);
                logger.LogInformation("Task '{Title}' assigned to user {UserId} on creation.", title, chosenUserId);
            }
        }

        await taskRepository.AddAsync(task);
        await taskRepository.SaveChangesAsync();

        logger.LogInformation("Task '{Title}' created with ID: {TaskId}", title, task.Id);
        return task;
    }

    public async Task ReassignTasksAsync()
    {
        var tasks = (await GetAllAsync()).Where(t => t.State != TaskState.Completed);
        var allUsers = await userService.GetAllAsync();

        foreach (var task in tasks)
        {
            var history = await taskRepository.GetHistoryAsync(task.Id);

            var currentUserId = task.AssignedUserId;
            var previousUserId = history.Skip(1).FirstOrDefault()?.UserId;

            var pastUserIds = history.Select(h => h.UserId).Distinct().ToList();
            var allUserIds = allUsers.Select(u => u.Id).ToList();

            var assignedToAll = allUserIds.All(id => pastUserIds.Contains(id));
            if (assignedToAll)
            {
                UnassignTask(task, TaskState.Completed);
                logger.LogInformation("Task '{TaskId}' marked as Completed. All users had it assigned.", task.Id);
                continue;
            }

            var eligibleUsers = allUsers
                .Where(u => u.Id != currentUserId && u.Id != previousUserId && !pastUserIds.Contains(u.Id))
                .ToList();

            if (eligibleUsers.Count == 0)
            {
                UnassignTask(task, TaskState.Waiting);
                logger.LogInformation("Task '{TaskId}' left unassigned due to no eligible users.", task.Id);
                continue;
            }

            var newUser = eligibleUsers.OrderBy(_ => Guid.NewGuid()).First();
            await AssignTask(task, newUser.Id);
            logger.LogInformation("Task '{TaskId}' reassigned to user {UserId}.", task.Id, newUser.Id);
        }

        await taskRepository.SaveChangesAsync();
    }

    public async Task<List<TaskItem>> GetAllAsync()
    {
        return await taskRepository.GetAllWithUserAsync();
    }

    public async Task<List<TaskTransferHistory>> GetTaskHistoryAsync(Guid taskId)
    {
        return await taskRepository.GetHistoryAsync(taskId);
    }


    #region Private

    private async Task ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required!");

        if (await taskRepository.TitleExistsAsync(title))
            throw new InvalidOperationException("Task with this title already exists!");
    }

    private async Task AssignTask(TaskItem task, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(task);
        task.AssignedUserId = userId;
        task.State = TaskState.InProgress;

        await taskRepository.AddTaskHistoryAsync(task.Id, userId);
        await taskRepository.SaveChangesAsync();
    }

    private static void UnassignTask(TaskItem task, TaskState state)
    {
        ArgumentNullException.ThrowIfNull(task);
        task.AssignedUserId = null;
        task.State = state;
    }

    private async Task<Guid> GetUserIdForNewTaskAsync(List<User> allUsers)
    {
        var allTasks = await taskRepository.GetAllAsync();
        var taskCounts = allTasks
            .Where(t => t.AssignedUserId != null)
            .GroupBy(t => t.AssignedUserId!.Value)
            .Select(g => new { UserId = g.Key, Count = g.Count() })
            .ToDictionary(x => x.UserId, x => x.Count);

        var usersWithZeroTasks = allUsers.Where(u => !taskCounts.ContainsKey(u.Id)).ToList();

        return usersWithZeroTasks.Count > 0
            ? usersWithZeroTasks.OrderBy(_ => Guid.NewGuid()).First().Id
            : Guid.Empty;
    }

    #endregion
}