using Entities;

namespace ServiceContracts
{
    /// <summary>
    /// Represents a service for managing tasks.
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Retrieves all tasks asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the collection of tasks.</returns>
        Task<IEnumerable<TaskItem>> GetAllTasksAsync();

        /// <summary>
        /// Retrieves a task by its ID asynchronously.
        /// </summary>
        /// <param name="taskId">The ID of the task to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the task with the specified ID.</returns>
        Task<TaskItem> GetTaskByIdAsync(int taskId);

        /// <summary>
        /// Adds a new task asynchronously.
        /// </summary>
        /// <param name="task">The task to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddTaskAsync(TaskItem task);

        /// <summary>
        /// Updates an existing task asynchronously.
        /// </summary>
        /// <param name="task">The task to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateTaskAsync(TaskItem task);

        /// <summary>
        /// Deletes a task by its ID asynchronously.
        /// </summary>
        /// <param name="taskId">The ID of the task to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteTaskAsync(int taskId);
    }
}
