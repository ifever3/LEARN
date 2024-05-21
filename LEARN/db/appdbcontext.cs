using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using LEARN.model;
using System.Reflection;

namespace LEARN.db
{
    public class appdbcontext : DbContext
    {
        private readonly mysqloption _mysqloptions;

        // public DbSet<Staff> Staff { get; set; }

        public appdbcontext(IOptions<mysqloption> mysqloptions)
        {
            _mysqloptions = mysqloptions.Value;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_mysqloptions.ConnectionString, new MySqlServerVersion(_mysqloptions.Version),
                    mysqlOptions =>
                        mysqlOptions.EnableRetryOnFailure(10,
                            TimeSpan.FromSeconds(60),
                            null))
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine);
            //.UseSnakeCaseNamingConvention();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           // modelBuilder.ApplyConfigurationsFromAssembly(typeof(StaffEntityTypeConfiguration).Assembly);
            // 应用程序集中的所有实体类型配置
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //单独的实体类型配置
            //modelBuilder.Entity<Staff>(entity =>
            //{
            //    entity.ToTable("staff");
            //    // 其他实体配置...
            //});
        }
    }
}
