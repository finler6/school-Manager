using SchoolManage.DAL.Entities;
using SchoolManage.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace SchoolManage.DAL.Seeds;

public static class EvaluationSeeds
{
   public static readonly EvaluationEntity one = new()
   {
      Id = Guid.NewGuid(),
      Points = "10",
      Description = "First evaluation",
      Activities = ActivitySeeds.one,
      ActivitiesId = ActivitySeeds.one.Id,
      Students = StudentSeeds.student1,
      StudentsId = StudentSeeds.student1.Id
   };

    public static readonly EvaluationEntity two = new()
    {
        Id = Guid.NewGuid(),
        Points = "9",
        Description = "Second evaluation",
        Activities = ActivitySeeds.two,
        ActivitiesId = ActivitySeeds.two.Id,
        Students = StudentSeeds.student2,
        StudentsId = StudentSeeds.student2.Id
    };

    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EvaluationEntity>().HasData(
            one with { Activities = null!, Students = null! },
            two with { Activities = null!, Students = null! }
        );
    }
}