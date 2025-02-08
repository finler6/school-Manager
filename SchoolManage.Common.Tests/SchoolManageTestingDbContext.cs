using SchoolManage.Common.Tests.Seeds;
using SchoolManage.DAL;
using Microsoft.EntityFrameworkCore;

namespace SchoolManage.Common.Tests;

public class SchoolManageTestingDbContext(DbContextOptions contextOptions, bool seedTestingData = false)
    : SchoolManageDbContext(contextOptions, seedDemoData: false)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (seedTestingData)
        {
            StudentSeeds.Seed(modelBuilder);
            SubjectSeeds.Seed(modelBuilder);
            ActivitySeeds.Seed(modelBuilder);
            EvaluationSeeds.Seed(modelBuilder);
        }
    }
}