using TaskManagerApiV2.Domain.Entities;

namespace TaskManagerApiV2.Domain.Abstractions;

public interface ITaskService
{
    Task ReassignTasksAsync();
    Task<TaskItem> CreateAsync(string title);
    Task<List<TaskItem>> GetAllAsync();
    Task<List<TaskTransferHistory>> GetTaskHistoryAsync(Guid taskId);
}