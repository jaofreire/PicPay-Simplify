using Microsoft.EntityFrameworkCore;
using PicPaySimplify.Map;
using PicPaySimplify.Models;

namespace PicPaySimplify.Data
{
    public class PicPayDbContext : DbContext
    {
        public PicPayDbContext(DbContextOptions<PicPayDbContext> options) : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<TransactionModel> Transactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new TransactionMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
