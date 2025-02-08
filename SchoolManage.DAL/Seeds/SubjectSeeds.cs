using SchoolManage.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace SchoolManage.DAL.Seeds;

public static class SubjectSeeds
{
    public static readonly SubjectEntity math = new()
    {
        Id = Guid.NewGuid(),
        Name = "Math",
        Acronym = "MTH"
    };

    public static readonly SubjectEntity english = new()
    {
        Id = Guid.NewGuid(),
        Name = "English",
        Acronym = "ENG"
    };

    public static void Seed(ModelBuilder modelBuilder) => 
        modelBuilder.Entity<SubjectEntity>().HasData(math, english);

}