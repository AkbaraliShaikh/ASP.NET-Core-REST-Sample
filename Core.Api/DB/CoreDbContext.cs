using Microsoft.EntityFrameworkCore;
using Core.Api.Models;

namespace Core.Api.DB
{
    public class CoreDbContext : DbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<Country> Country { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().HasKey(x => x.Id);

            base.OnModelCreating(modelBuilder);

        }
    }
}
