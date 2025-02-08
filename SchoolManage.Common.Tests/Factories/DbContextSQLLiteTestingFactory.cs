using SchoolManage.DAL;
using Microsoft.EntityFrameworkCore;

namespace SchoolManage.Common.Tests.Factories;

public class DbContextSqLiteTestingFactory(string databaseName, bool seedTestingData = false)
    : IDbContextFactory<SchoolManageDbContext>
{
    public SchoolManageDbContext CreateDbContext()
    {
        DbContextOptionsBuilder<SchoolManageDbContext> builder = new();
        builder.UseSqlite($"Data Source={databaseName};Cache=Shared");

        // builder.LogTo(Console.WriteLine); //Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
        // builder.EnableSensitiveDataLogging();

        return new SchoolManageTestingDbContext(builder.Options, seedTestingData);
    }
}