using SchoolManage.BL.Mappers;
using SchoolManage.Common.Tests;
using SchoolManage.Common.Tests.Factories;
using SchoolManage.DAL;
using SchoolManage.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace SchoolManage.BL.Tests;

public class FacadeTestsBase : IAsyncLifetime
{
    protected FacadeTestsBase(ITestOutputHelper output)
    {
        XUnitTestOutputConverter converter = new(output);
        Console.SetOut(converter);

        DbContextFactory = new DbContextSqLiteTestingFactory(GetType().FullName!, seedTestingData: true);

        ActivityModelMapper = new ActivityModelMapper();
        EvaluationModelMapper = new EvaluationModelMapper();
        StudentModelMapper = new StudentModelMapper();
        SubjectModelMapper = new SubjectModelMapper();

        UnitOfWorkFactory = new UnitOfWorkFactory(DbContextFactory);
    }

    protected IDbContextFactory<SchoolManageDbContext> DbContextFactory { get; }
    protected ActivityModelMapper ActivityModelMapper { get; }
    protected EvaluationModelMapper EvaluationModelMapper { get; }
    protected StudentModelMapper StudentModelMapper { get; }
    protected SubjectModelMapper SubjectModelMapper { get; }
    protected UnitOfWorkFactory UnitOfWorkFactory { get; }

    public async Task InitializeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeletedAsync();
        await dbx.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeletedAsync();
    }
}