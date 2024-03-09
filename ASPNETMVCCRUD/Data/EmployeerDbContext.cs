using EmployersEnvironment.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace EmployersEnvironment.Data
{
    public class EmployeerDbContext : DbContext
    {
        public EmployeerDbContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<Employee> Employees { get; set; }

        public DbSet<Models.Domain.Task> Tasks { get; set; }

    }
}
