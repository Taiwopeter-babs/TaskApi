using Microsoft.EntityFrameworkCore;

namespace TaskApi.Models;

public class TaskContext : DbContext
{
    public TaskContext(DbContextOptions<TaskContext> options) : base(options)
    { }

    public DbSet<TaskItem> TaskItems { get; set; } = null!;
    public DbSet<StudentItem> StudentItems { get; set; } = null!;

}