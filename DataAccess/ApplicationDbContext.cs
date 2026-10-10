using Microsoft.EntityFrameworkCore;

namespace ToDoList.DataAccess
{
    public class ApplicationDbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=.;Initial Catalog=ToDoList;" +
            "Trusted_Connection=true;" +
            "TrustServerCertificate=true;Encrypt=true");
        }
        public DbSet<ToDoList.Models.Task> Tasks { set; get; }
    }
}
