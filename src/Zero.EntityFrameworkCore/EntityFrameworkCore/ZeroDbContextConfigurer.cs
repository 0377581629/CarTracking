using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Zero.EntityFrameworkCore
{
    public static class ZeroDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<ZeroDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<ZeroDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}