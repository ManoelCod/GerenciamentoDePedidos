using Microsoft.EntityFrameworkCore;
using Task = TaskManagement.Domain.Models.Task;

namespace TaskManagement.Infrastructure.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }

        public DbSet<Task> Tasks { get; set; }
    }
}
