using Microsoft.EntityFrameworkCore;

namespace TODO_LIST.Models
{
    public class TodoListContext:DbContext
    {
        public TodoListContext(DbContextOptions<TodoListContext> options) :base(options)
        {
               
        }

        public DbSet<TodoList> TodoLists { get; set; }
    }
}
