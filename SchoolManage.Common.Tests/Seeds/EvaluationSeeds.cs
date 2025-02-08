using System;
using SchoolManage.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace SchoolManage.Common.Tests.Seeds;

public static class EvaluationSeeds
{
    public static readonly EvaluationEntity EmptyEvaluation = new()
    {
        Id = default,
        Points = default,
        Description = default!,
        Activities = default!,
        ActivitiesId = default!,
        Students = default!,
        StudentsId = default!
    };

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

    //To ensure that no tests reuse these clones for non-idempotent operations
    public static readonly EvaluationEntity IngredientAmountEntityUpdate = one with { Id = Guid.Parse("A2E6849D-A158-4436-980C-7FC26B60C674"), Activities = null!, Students = null! };
    public static readonly EvaluationEntity IngredientAmountEntityDelete = two with { Id = Guid.Parse("30872EFF-CED4-4F2B-89DB-0EE83A74D279"), Activities = null!, Students = null! };

    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EvaluationEntity>().HasData(
            one with { Activities = null!, Students = null! },
            two with { Activities = null!, Students = null! },
            IngredientAmountEntityUpdate with { Activities = null!, Students = null! },
            IngredientAmountEntityDelete with { Activities = null!, Students = null! }
        );
    }
}
