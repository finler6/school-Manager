using System;
using SchoolManage.Common.Enums;
using SchoolManage.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace SchoolManage.Common.Tests.Seeds;

public static class SubjectSeeds
{
    public static readonly SubjectEntity EmptySubject = new()
    {
        Id = default,
        Name = default!,
        Acronym = default!,
    };

    public static readonly SubjectEntity math = new()
    {
        Id = Guid.NewGuid(),
        Name = "Math",
        Acronym = "MTH",
    };

    public static readonly SubjectEntity english = new()
    {
        Id = Guid.NewGuid(),
        Name = "English",
        Acronym = "ENG",
    };

    //To ensure that no tests reuse these clones for non-idempotent operations
    public static readonly SubjectEntity SubjectEntityUpdate = math with { Id = Guid.Parse("0953F3CE-7B1A-48C1-9796-D2BAC7F67868"), Students = Array.Empty<StudentEntity>(), Activities = Array.Empty<ActivityEntity>() };
    public static readonly SubjectEntity SubjectEntityDelete = english with { Id = Guid.Parse("5DCA4CEA-B8A8-4C86-A0B3-FFB78FBA1A09"), Students = Array.Empty<StudentEntity>(), Activities = Array.Empty<ActivityEntity>() };

    static SubjectSeeds()
    {
        math.Activities.Add(ActivitySeeds.one);
        english.Activities.Add(ActivitySeeds.two);
        math.Students.Add(StudentSeeds.student1);
        english.Students.Add(StudentSeeds.student1);
    }

    public static void Seed(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<SubjectEntity>().HasData(
            math with { Activities = Array.Empty<ActivityEntity>(), Students = Array.Empty<StudentEntity>() },
            english with { Activities = Array.Empty<ActivityEntity>(), Students = Array.Empty<StudentEntity>() },
            SubjectEntityUpdate,
            SubjectEntityDelete
            );
}