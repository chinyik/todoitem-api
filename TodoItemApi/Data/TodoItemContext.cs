using Microsoft.EntityFrameworkCore;
using TodoItemApi.Models;

namespace TodoItemApi.Data
{
    public class TodoItemContext : DbContext
    {
        public TodoItemContext(DbContextOptions<TodoItemContext> options) : base(options)
            { }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
