using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TaskItem> Tasks { get; set; }

        public async Task UpdateTask_Sp(TaskItem task)
        {
            await this.Database.ExecuteSqlRawAsync("EXEC sp_UpdateTask @TaskId = {0}, @Title = {1}, @Description = {2}, @IsCompleted = {3}",
                task.TaskId, task.Title, task.Description, task.IsCompleted);
        }
    }
}
