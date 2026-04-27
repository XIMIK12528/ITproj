using DbModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccsess.Context
{
    public class Context : DbContext
    {
        public DbSet<AccountDb> Accounts { get; set; }
        public DbSet<RefreshTokenDb> RefreshTokens { get; set; }

        public Context()
        {
        }

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
