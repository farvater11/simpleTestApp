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
    }
}