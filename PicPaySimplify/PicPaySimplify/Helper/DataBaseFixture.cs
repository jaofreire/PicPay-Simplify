using Microsoft.EntityFrameworkCore;
using PicPaySimplify.Data;

namespace PicPaySimplify.Helper
{
    public class DataBaseFixture
    {

        private static object _lock = new object();

        public DataBaseFixture()
        {
            lock (_lock)
            {
                using var context = CreateContext();
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.SaveChanges();
            }
           
        }

        public PicPayDbContext CreateContext()

        {
            return new PicPayDbContext(new DbContextOptionsBuilder<PicPayDbContext>().UseSqlServer(Environment.GetEnvironmentVariable("DataBaseSql")).Options);
        }

    }
}
