using System.Threading.Tasks;
using SchoolManage.Common.Tests;
using SchoolManage.Common.Tests.Factories;
using Microsoft.EntityFrameworkCore;
using Xunit;
// using Xunit.Abstractions;

namespace SchoolManage.DAL.Tests;

public class DbContextTestsBase : IAsyncLifetime
{
    protected DbContextTestsBase()
    {
        DbContextFactory = new DbContextSqLiteTestingFactory(GetType().FullName!, seedTestingData: true);
        SchoolManageDbContextSUT = DbContextFactory.CreateDbContext();
    }

    protected IDbContextFactory<SchoolManageDbContext> DbContextFactory { get; }
    protected SchoolManageDbContext SchoolManageDbContextSUT { get; }


    public async Task InitializeAsync()
    {
        await SchoolManageDbContextSUT.Database.EnsureDeletedAsync();
        await SchoolManageDbContextSUT.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await SchoolManageDbContextSUT.Database.EnsureDeletedAsync();
        await SchoolManageDbContextSUT.DisposeAsync();
    }
}