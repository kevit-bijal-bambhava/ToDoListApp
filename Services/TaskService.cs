using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceContracts;

namespace Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<TaskService> _logger;

        public TaskService(AppDbContext dbContext, ILogger<TaskService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            try
            {
                return await _dbContext.Tasks.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all tasks");
                throw;
            }
        }

        public async Task<TaskItem> GetTaskByIdAsync(int taskId)
        {
            try
            {
                return await _dbContext.Tasks.FindAsync(taskId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting task by ID: {TaskId}", taskId);
                throw;
            }
        }

        public async Task AddTaskAsync(TaskItem task)
        {
            try
            {
                await _dbContext.Tasks.AddAsync(task);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a task");
                throw;
            }
        }

        public async Task UpdateTaskAsync(TaskItem task)
        {
            try
            {
                // Attach the task to the context and mark it as modified
                _dbContext.Tasks.Attach(task);
                _dbContext.Entry(task).State = EntityState.Modified;

                // Save the changes to the database
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating task with ID: {TaskId}", task.Id);
                throw;
            }
        }

        public async Task DeleteTaskAsync(int taskId)
        {
            try
            {
                var task = await _dbContext.Tasks.FindAsync(taskId);
                if (task != null)
                {
                    _dbContext.Tasks.Remove(task);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting task with ID: {TaskId}", taskId);
                throw;
            }
        }
    }
}
