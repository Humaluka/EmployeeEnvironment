using HelpComing.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace HelpComing.Data
{
    public class HelpComingDbContext : DbContext
    {
        public HelpComingDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Request> Requests { get; set; }

        public DbSet<Reply> Replies { get; set; }
        public DbSet<Country> Countries { get; set; }

        // public DbSet<Models.Domain.Task> Tasks { get; set; }

    }
}
