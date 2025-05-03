using Microsoft.EntityFrameworkCore;
using TaskManagerApiV2.Application.Abstractions;
using TaskManagerApiV2.Domain.Entities;
using TaskManagerApiV2.Domain.Enums;

namespace TaskManagerApiV2.Persistence.Repositories;

public class TaskRepository(AppDbContext dbContext, IUserRepository userRepository)
    : RepositoryBase<TaskItem>(dbContext), ITaskRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<bool> TitleExistsAsync(string title, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tasks.AnyAsync(t => t.Title == title, cancellationToken);
    }

    public async Task<List<TaskItem>> GetAllWithUserAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tasks.Include(t => t.AssignedUser).ToListAsync(cancellationToken);
    }

    public async Task<List<TaskItem>> GetUncompletedAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tasks
            .Where(t => t.State != TaskState.Completed)
            .ToListAsync(cancellationToken);
    }

    public async Task AddTaskHistoryAsync(Guid taskId, Guid userId)
    {
        await _dbContext.TaskTransferHistories.AddAsync(new TaskTransferHistory
        {
            TaskId = taskId,
            UserId = userId
        });
    }

    public async Task<List<TaskTransferHistory>> GetHistoryAsync(Guid taskId,
        CancellationToken cancellationToken = default)
    {
        var history = await _dbContext.TaskTransferHistories
            .Where(h => h.TaskId == taskId)
            .OrderByDescending(h => h.TransferredAt)
            .ToListAsync(cancellationToken);

        return (from h in history
            join u in await userRepository.GetAllAsync(cancellationToken) on h.UserId equals u.Id
            select new TaskTransferHistory
            {
                UserId = u.Id,
                TaskId = h.TaskId,
                TransferredAt = h.TransferredAt
            }).ToList();
    }
}