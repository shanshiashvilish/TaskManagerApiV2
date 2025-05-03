using TaskManagerApiV2.Domain.Entities;

namespace TaskManagerApiV2.Application.Abstractions;

public interface ITaskRepository : IRepositoryBase<TaskItem>
{
    Task<bool> TitleExistsAsync(string title, CancellationToken cancellationToken = default);
    Task<List<TaskItem>> GetAllWithUserAsync(CancellationToken cancellationToken = default);
    Task<List<TaskItem>> GetUncompletedAsync(CancellationToken cancellationToken = default);
    Task<List<TaskTransferHistory>> GetHistoryAsync(Guid taskId, CancellationToken cancellationToken = default);
    Task AddTaskHistoryAsync(Guid taskId, Guid userId);
}