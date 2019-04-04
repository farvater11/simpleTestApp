namespace SimpleTestApp.Model
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class SimpleTestAppContext : DbContext
    {
        public SimpleTestAppContext()
            : base("name=SimpleTestAppContext.cs")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ToDoTaskList> ToDoTaskLists { get; set; }
        public DbSet<ToDoTask> ToDoTasks { get; set; }
    }
}